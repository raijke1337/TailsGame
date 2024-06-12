using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units.Stats;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.Units
{
    [RequireComponent(typeof(ControlInputsBase))]

    public abstract class BaseUnit : MonoBehaviour, ITakesTriggers, IHasEffects
    {
        protected Animator _animator;
        [SerializeField] protected RuntimeAnimatorController _baseAnimator;

        protected Rigidbody _rigidbody;
        public Collider GetCollider { get; private set; }
        protected ControlInputsBase _controller;

        public Side Side;
        public BaseStatsConfig StatsConfig;
        public ItemEmpties GetEmpties => _controller.GetEmpties;

        public abstract ReferenceUnitType GetUnitType();
        public IReadOnlyDictionary<BaseStatType, StatValueContainer> GetStats => _controller.GetStatsController.GetBaseStats;

        public T GetInputs<T>() where T : ControlInputsBase => _controller as T;
        public ControlInputsBase GetInputs()
        {
            return _controller;
        }

        private bool _locked = false;
        public bool LockUnit
        {
            get
            {
                return _locked;
            }
            set
            {
                _locked = value;
                ZeroAnimatorFloats();
                _controller.LockInputs = value;
                if (value)
                {
                    AnimateMovement(); // to reset the movement anim
                }
            }
        }

        public string GetFullName => _controller.GetStatsController.GetDisplayName;
        public bool IsUnitAlive { get; protected set; } = true;

        public event SimpleEventsHandler<BaseUnit> BaseUnitDiedEvent;
        public event SkillRequestedEvent SkillRequestFromInputsSuccessEvent;


        #region equipments
        public UnitInventoryComponent GetUnitInventory { get; protected set; }
        public void AddItem(Item item,bool equip)
        {
            GetUnitInventory.PickedUpItem(item, equip);
            OnItemAdd(item);
        }
        protected abstract void InitInventory();
        protected abstract void OnItemAdd(Item i);

        protected void CreateStartingEquipments(UnitInventoryComponent item) // equipment can't be changed mid-level so it's no problem here that this is run once
        {
            UpdateComponents();
            _controller.AssignDefaultItems(item);
        }
        public bool IsArmed
        {
            get
            {
                var list = new List<EquipmentItem>(GetUnitInventory.GetCurrentEquips);
                return list.Any(t => t.ItemType == EquipItemType.MeleeWeap) || list.Any(t => t.ItemType == EquipItemType.RangedWeap);
            }
        }

        #endregion

        #region managed
        public virtual void InitiateUnit() // this is run by unit manager
        {
            UpdateComponents();
            _controller.SetUnit(this);
            _controller.StartController();
            InitInventory();

            _animator.runtimeAnimatorController = _baseAnimator;
            _animator.Play("Idle");

            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                ControllerEventsBinds(true); 
                LockUnit = false;
            }                       

        }

        public virtual void DisableUnit()
        {
            ControllerEventsBinds(false);
            _controller.StopController();
            StopAllCoroutines();
        }


        public virtual void RunUpdate(float delta)
        {
            if (_controller == null)
            {
                Debug.LogWarning($"Unit {this} was not initialized!");
                return;
            }

            if (LockUnit) return;

            AnimateMovement();
            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            _controller.UpdateController(delta);

            if (_controller.LockInputs) return;

            UnitMovement(delta);
            UnitRotation(delta);
        }

        #endregion


        #region unit

        protected virtual void UpdateComponents()
        {
            if (GetCollider == null) GetCollider = GetComponent<Collider>();
            if (_animator == null) _animator = GetComponent<Animator>();
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
            if (_controller == null) _controller = GetComponent<ControlInputsBase>();
            if (_groundCollider == null) _groundCollider = GetComponent<GroundDetectorPlatformCollider>();
        }


        protected virtual void ControllerEventsBinds(bool isEnable)
        {
            if (isEnable)
            {
                _controller.CombatActionSuccessEvent += (t) => AnimateCombatActivity(t);
                _controller.StaggerHappened += AnimateStagger;
                _controller.SkillSpawnEvent += OnInputsCreateSkill;
                _controller.EffectEventRequest += EffectEventCallback;
                _controller.TriggerEventRequest += TriggerEventCallback;
                _controller.SpawnProjectileEvent += PlaceProjectileCallback;
                _controller.JumpCalledEvent += StartJump;
                _controller.ZeroHealthHappened += OnInputsReportDeath;
                _controller.DamageHappened += OnInputsReportDamage;

                if (_groundCollider != null)
                {
                    _groundCollider.PlatfromCollidedWithTagEvent += LandJump;
                }
                        

            }
            else
            {

                _controller.CombatActionSuccessEvent -= (t) => AnimateCombatActivity(t);
                _controller.StaggerHappened -= AnimateStagger;
                _controller.SkillSpawnEvent -= OnInputsCreateSkill;
                _controller.EffectEventRequest -= EffectEventCallback;
                _controller.TriggerEventRequest -= TriggerEventCallback;
                _controller.SpawnProjectileEvent -= PlaceProjectileCallback;
                _controller.JumpCalledEvent -= StartJump;
                _controller.ZeroHealthHappened -= OnInputsReportDeath;
                _controller.DamageHappened -= OnInputsReportDamage;

                if (_groundCollider != null)
                {
                    _groundCollider.PlatfromCollidedWithTagEvent -= LandJump;
                }

            }

        }

        private void OnInputsReportDamage(float arg)
        {
            HandleDamage(arg);
            if (_controller.DebugMessage)
            {
                Debug.Log($" {GetFullName} hp change {-arg}");
            }

        }

        protected void OnInputsCreateSkill(SkillProjectileComponent data, BaseUnit source, Transform where)
        {
            SkillRequestFromInputsSuccessEvent?.Invoke(data, source, where);
        }

        #endregion

        #region stats
        private void OnInputsReportDeath()
        {
            if (_controller.DebugMessage)
            {
                Debug.Log($"{GetFullName} died");
            }

            HandleDeath();
            IsUnitAlive = false;
            _animator.SetTrigger("Death");
            _controller.LockInputs = true;

            GetCollider.enabled = false;
            _rigidbody.useGravity = false;

            BaseUnitDiedEvent?.Invoke(this);

        }

        protected abstract void HandleDamage(float value);
        protected abstract void HandleDeath();

        protected Vector2 animVect = Vector2.zero;
        protected abstract void StartJump();
        protected abstract void LandJump(string tag);


        [SerializeField] protected float _debugJumpForceMult;
        [SerializeField] private GroundDetectorPlatformCollider _groundCollider;

        public virtual void ApplyEffect(TriggeredEffect eff)
        {
            _controller.ApplyEffect(eff);
        }
        #endregion
        #region movement, animations

        [Space, SerializeField, Range(1, 0)] protected float _minAngleToPlayRotation;
        [SerializeField, Range(0, 1)] protected float _dampTime = 0.1f;

        protected virtual void UnitMovement(float delta)
        {
            transform.position += delta * _controller.GetMoveDirection
                * GetStats[BaseStatType.MoveSpeed].GetCurrent;
        }
        protected Vector3 _sDampVel;
        protected virtual void UnitRotation(float delta)
        {
            if (_controller.GetRotationDot < _minAngleToPlayRotation)
            {
                transform.LookAt(Vector3.SmoothDamp(transform.position,_controller.GetAimDirection, ref _sDampVel, _dampTime));
            }
        }

        public void UnitDodge(BoosterSkillInstanceComponent bs)
        {
            if (LockUnit || _controller.LockInputs) return;
            else
            {
                if (_controller.DebugMessage)
                {
                    Debug.Log($"{GetFullName} dodge action");
                }
                _rigidbody.AddForce(_controller.GetAimDirection * bs.GetDodgeSettings.Range*10, ForceMode.Impulse);
            }
        }


        protected virtual void ZeroAnimatorFloats()
        {
            _animator.SetBool("Moving", false);
            _animator.SetFloat("ForwardMove", 0f);
            _animator.SetFloat("SideMove", 0f);
            _animator.SetFloat("Rotation", 0);

        }
        protected virtual void AnimateMovement()
        {
            Vector3 movement = _controller.GetMoveDirection;

            if (movement.x == 0f && movement.z == 0f)
            {
                _animator.SetBool("Moving", false);
                _animator.SetFloat("ForwardMove", 0f);
                _animator.SetFloat("SideMove", 0f);

                // rotate in place
                if (_controller.GetRotationDot < _minAngleToPlayRotation)
                {
                    _animator.SetFloat("Rotation", _controller.GetRotationDirection);
                }
                else
                {
                    _animator.SetFloat("Rotation", 0);
                }
            }
            else
            {
                _animator.SetFloat("Rotation",0);
                _animator.SetBool("Moving", true);

                UpdateAnimatorVector(movement);

                _animator.SetFloat("ForwardMove", animVect.y);
                _animator.SetFloat("SideMove", animVect.x);

            }
        }

        //  calculates the vector which is used to set values in animator
        protected void UpdateAnimatorVector(Vector3 movement)
        {
            var playerFwd = transform.forward;
            var playerRght = transform.right;
            movement.y = 0;
            movement.Normalize();
            // Dot product of two vectors determines how much they are pointing in the same direction.
            // If the vectors are normalized (transform.forward and right are)
            // then the value will be between -1 and +1.
            var x = Vector3.Dot(playerRght, movement);
            var z = Vector3.Dot(playerFwd, movement);
            animVect.x = x;
            animVect.y = z;
        }


        protected virtual void AnimateCombatActivity(CombatActionType type)
        {
            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            switch (type)
            {
                case CombatActionType.Melee:
                    _animator.SetTrigger("MeleeAttack");
                    break;
                case CombatActionType.Ranged:
                    _animator.SetTrigger("RangedAttack");
                    break;
                case CombatActionType.Dodge:
                    _animator.SetTrigger("Dodge");
                   // _controller.PerformDodging(); //old Dodge.
                    //SkillRequestCallBack(_controller.GetSkillsController.GetSkillDataByType(type), this);
                    break;
                case CombatActionType.MeleeSpecialQ:
                    _animator.SetTrigger("MeleeSpecial");
                    //SkillRequestCallBack(_controller.GetSkillsController.GetSkillDataByType(type), this);
                    break;
                case CombatActionType.RangedSpecialE:
                    _animator.SetTrigger("RangedSpecial");
                    //SkillRequestCallBack(_controller.GetSkillsController.GetSkillIDByType(type), this);
                    break;
                case CombatActionType.ShieldSpecialR:
                    _animator.SetTrigger("ShieldSpecial");
                    //SkillRequestCallBack(_controller.GetSkillsController.GetSkillIDByType(type), this);
                    break;
            }
        }

        public virtual void TriggerTogglingEvent_UE(float value)
        {    // 1 on start 0 on end
            if (_controller.DebugMessage)
            {
                Debug.Log($"Animation event triggers toggle {value}");
            }

            if (_controller == null) return; //in case we have a scene

            bool result = value > 0;
            _controller.GetWeaponController.ToggleTriggersOnMelee(result);
        }

        protected virtual void AnimateStagger()
        {
            _animator.SetTrigger("TakeDamage");
           // Debug.Log($"{GetFullName} got stunned!");
        }


        #endregion



        #region trigger events
        public event TriggerEvent UnitTriggerRequestEvent;
        protected void TriggerEventCallback(BaseUnit tg, BaseUnit src, bool ent, TriggeredEffect cfg)
        {
            UnitTriggerRequestEvent?.Invoke(tg, src, ent, cfg);
        }

        #endregion


        #region effects

        public event EffectsManagerEvent BaseControllerEffectEvent;
        protected void EffectEventCallback(EffectRequestPackage pack) => BaseControllerEffectEvent?.Invoke(pack);
        #endregion


        #region projectiles
        public event SimpleEventsHandler<ProjectileComponent, BaseUnit> UnitPlacedProjectileEvent;
        protected void PlaceProjectileCallback(ProjectileComponent comp)
        {
            UnitPlacedProjectileEvent?.Invoke(comp, this);
        }
        #endregion

        


    }
}