using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units.Inputs;
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
        


        public ItemEmpties GetEmpties => _controller.GetEmpties;

        public abstract ReferenceUnitType GetUnitType();

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
                    UpdateAnimator(); // to reset the movement anim
                }
            }
        }


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
                return list.Any(t => t.ItemType == EquipmentType.MeleeWeap) || list.Any(t => t.ItemType == EquipmentType.RangedWeap);
            }
        }

        #endregion

        #region managed
        public virtual void InitiateUnit() // this is run by unit manager
        {
            UpdateComponents();
            _controller.Unit = this;
            _controller.StartController();
            InitInventory();

            _controller.GetGroundCollider.OffTheGroundEvent += HandleAirTime;

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
            _controller.GetGroundCollider.OffTheGroundEvent -= HandleAirTime;
        }


        public virtual void RunUpdate(float delta)
        {
            if (LockUnit) return;
            UpdateAnimator();
            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            _controller.UpdateController(delta);

        }

        #endregion


        #region unit

        protected virtual void UpdateComponents()
        {
            if (GetCollider == null) GetCollider = GetComponent<Collider>();
            if (_animator == null) _animator = GetComponent<Animator>();
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
            if (_controller == null) _controller = GetComponent<ControlInputsBase>();
        }


        protected virtual void ControllerEventsBinds(bool isEnable)
        {
            if (isEnable)
            {
                _controller.CombatActionAnimationRequest += (t) => AnimateCombatActivity(t);
                _controller.StaggerHappened += AnimateStagger;
                _controller.SkillSpawnEvent += OnInputsCreateSkill;
                _controller.EffectEventRequest += EffectEventCallback;
                _controller.TriggerEventRequest += TriggerEventCallback;
                _controller.SpawnProjectileEvent += PlaceProjectileCallback;
                _controller.ZeroHealthHappened += OnInputsReportDeath;
                _controller.DamageHappened += OnInputsReportDamage;     

            }
            else
            {

                _controller.CombatActionAnimationRequest -= (t) => AnimateCombatActivity(t);
                _controller.StaggerHappened -= AnimateStagger;
                _controller.SkillSpawnEvent -= OnInputsCreateSkill;
                _controller.EffectEventRequest -= EffectEventCallback;
                _controller.TriggerEventRequest -= TriggerEventCallback;
                _controller.SpawnProjectileEvent -= PlaceProjectileCallback;
                _controller.ZeroHealthHappened -= OnInputsReportDeath;
                _controller.DamageHappened -= OnInputsReportDamage;
            }

        }

        private void OnInputsReportDamage(float arg)
        {
            EventBus<DrawDamageEvent>.Raise(new DrawDamageEvent(arg,this));
            HandleDamage(arg);
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
                Debug.Log($"{_controller.GetFullName} died");
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


        [SerializeField] protected float _debugJumpForceMult;

        public virtual void ApplyEffect(TriggeredEffect eff)
        {
            _controller.ApplyEffect(eff);
        }
        #endregion
        #region movement, animations


        public void UnitDodge(BoosterSkillInstanceComponent bs)
        {
            if (LockUnit || _controller.LockInputs) return;
            Debug.Log($"whoosh!");
        }


        protected virtual void ZeroAnimatorFloats()
        {
            _animator.SetBool("Moving", false);
            _animator.SetFloat("ForwardMove", 0f);
            _animator.SetFloat("SideMove", 0f);
            _animator.SetFloat("Rotation", 0);

        }

        Coroutine airCor;

        private void HandleAirTime(bool started)
        {
            if (started)
            {
                airCor = StartCoroutine(UpdateAirTimeValue());
            }
            if (!started && airCor!= null)
            {
                StopCoroutine(airCor);
                _animator.SetFloat("AirTime", 0);
                _animator.SetTrigger("JumpEnd");
            }
        }

        private IEnumerator UpdateAirTimeValue()
        { 
            
            while (true)
            {
                _animator.SetFloat("AirTime", _controller.GetGroundCollider.AirTime);
                yield return null;  
            }
        }

        protected virtual void UpdateAnimator()
        {
            Vector3 movement = _controller.GetMovementVector;

            if (movement.x == 0f && movement.z == 0f)
            {
                _animator.SetBool("Moving", false);
                _animator.SetFloat("ForwardMove", 0f);
                _animator.SetFloat("SideMove", 0f);

                // rotate in place
                if (_controller is InputsPlayer p)
                {
                    if (p.PlayerInputsDot < p.RotationTreschold)
                    {
                        _animator.SetFloat("Rotation", p.PlayerInputsCross);
                    }
                    else
                    {
                        _animator.SetFloat("Rotation", 0);
                    }
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


        protected virtual void AnimateCombatActivity(UnitActionType type)
        {
            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            switch (type)
            {
                case UnitActionType.Melee:
                    _animator.SetTrigger("MeleeAttack");
                    break;
                case UnitActionType.Ranged:
                    _animator.SetTrigger("RangedAttack");
                    break;
                case UnitActionType.DodgeSkill:
                    _animator.SetTrigger("Dodge");
                    break;
                case UnitActionType.MeleeSkill:
                    _animator.SetTrigger("MeleeSpecial");
                    break;
                case UnitActionType.RangedSkill:
                    _animator.SetTrigger("RangedSpecial");
                    break;
                case UnitActionType.ShieldSkill:
                    _animator.SetTrigger("ShieldSpecial");
                    break;
                case UnitActionType.Jump:
                    _animator.SetTrigger("JumpStart");
                    break;
            }
        }


        public virtual void TriggerTogglingEvent_UE(float value)
        {    
            if (_controller == null) return; //in case we have a scene

            bool result = value > 0;
            _controller.GetWeaponController.ToggleTriggersOnMelee(result);
        }

        protected virtual void AnimateStagger()
        {
            _animator.SetTrigger("TakeDamage");
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