using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units.Inputs;
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
        protected ControlInputsBase _inputs;

        public Side Side;
        


        public ItemEmpties GetEmpties => _inputs.GetEmpties;

        public abstract ReferenceUnitType GetUnitType();

        public T GetInputs<T>() where T : ControlInputsBase => _inputs as T;
        public ControlInputsBase GetInputs()
        {
            return _inputs;
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
                _inputs.LockInputs = value;
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
            _inputs.AssignDefaultItems(item);
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
            _inputs.Unit = this;
            _inputs.StartController();
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
            _inputs.StopController();
            StopAllCoroutines();
        }


        public virtual void RunUpdate(float delta)
        {
            if (LockUnit) return;
            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            _inputs.UpdateController(delta);

        }

        #endregion


        #region unit

        protected virtual void UpdateComponents()
        {
            if (GetCollider == null) GetCollider = GetComponent<Collider>();
            if (_animator == null) _animator = GetComponent<Animator>();
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
            if (_inputs == null) _inputs = GetComponent<ControlInputsBase>();
        }


        protected virtual void ControllerEventsBinds(bool isEnable)
        {
            if (isEnable)
            {
                _inputs.SkillSpawnEvent += OnInputsCreateSkill;
                _inputs.EffectEventRequest += EffectEventCallback;
                _inputs.TriggerEventRequest += TriggerEventCallback;
                _inputs.SpawnProjectileEvent += PlaceProjectileCallback;
                _inputs.ZeroHealthHappened += OnInputsReportDeath;
                _inputs.DamageHappened += OnInputsReportDamage;     

            }
            else
            {
                _inputs.SkillSpawnEvent -= OnInputsCreateSkill;
                _inputs.EffectEventRequest -= EffectEventCallback;
                _inputs.TriggerEventRequest -= TriggerEventCallback;
                _inputs.SpawnProjectileEvent -= PlaceProjectileCallback;
                _inputs.ZeroHealthHappened -= OnInputsReportDeath;
                _inputs.DamageHappened -= OnInputsReportDamage;
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
            if (_inputs.DebugMessage)
            {
                Debug.Log($"{_inputs.GetFullName} died");
            }

            HandleDeath();
            IsUnitAlive = false;

            _inputs.LockInputs = true;

            GetCollider.enabled = false;
            _rigidbody.useGravity = false;

            BaseUnitDiedEvent?.Invoke(this);

        }

        protected abstract void HandleDamage(float value);
        protected abstract void HandleDeath();

        public virtual void ApplyEffect(TriggeredEffect eff)
        {
            _inputs.ApplyEffect(eff);
        }
        #endregion

        public void UnitDodge(BoosterSkillInstanceComponent bs)
        {
            if (LockUnit || _inputs.LockInputs) return;
            Debug.Log($"whoosh!");
        }

        public virtual void TriggerTogglingEvent_UE(float value)
        {    
            if (_inputs == null) return; //in case we have a scene

            bool result = value > 0;
            _inputs.GetWeaponController.ToggleTriggersOnMelee(result);
        }






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