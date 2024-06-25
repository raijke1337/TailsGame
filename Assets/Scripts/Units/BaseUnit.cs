using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Skills;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units.Stats;
using KBCore.Refs;
using UnityEngine;


namespace Arcatech.Units
{
    [RequireComponent(typeof(ControlInputsBase),typeof(UnitStatsController),typeof(UnitInventoryComponent))]

    public abstract class BaseUnit : ValidatedMonoBehaviour
    {
        [Header("Unit settings")]
        public Side Side;
        [SerializeField] protected UnitItemsSO defaultEquips;
        [SerializeField] protected ItemEmpties itemEmpties;
        [SerializeField] protected BaseStatsConfig defaultStats;
        [SerializeField] protected MovementStatsConfig movementStats;
        [SerializeField] protected DrawItemsStrategy defaultItemsDrawStrat;
        

        [SerializeField] protected RuntimeAnimatorController _baseAnimator;
        [SerializeField,Self] protected Animator _animator;
        [SerializeField, Self] protected UnitInventoryComponent _inventory;
        [SerializeField, Self] protected UnitStatsController _stats;
        [SerializeField, Self] protected ControlInputsBase _inputs;

        public string GetUnitName => defaultStats.DisplayName;
        public UnitInventoryComponent GetInventoryComponent => _inventory;
        public abstract ReferenceUnitType GetUnitType();

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

        public void AddItem(Item item,bool equip)
        {
            _inventory.PickedUpItem(item, equip);
            OnItemAdd(item);
        }
        protected abstract UnitInventoryItemConfigsContainer SelectSerializedItemsConfig();
        protected abstract void OnItemAdd(Item i);

        #endregion

        #region managed
        public virtual void InitiateUnit() // this is run by unit manager
        {

            _inventory.LoadSerializedItems(SelectSerializedItemsConfig(),this)
                .SetItemEmpties(itemEmpties)
                .DrawItems(defaultItemsDrawStrat)
                .StartController();

            _stats.PopulateDictionary().
                AddMods(defaultStats.InitialStats)
                .AddMods(_inventory.GetCurrentMods)
                .StartController();

            _stats.StatsUpdatedEvent += HandleStatChangesEvents;
            
            _inputs.PopulateDictionary().
                SetMovementStats(movementStats)
                .StartController();
            

            _animator.runtimeAnimatorController = _baseAnimator;
            _animator.Play("Idle");

            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                LockUnit = false;
            }    
        }

        protected virtual void HandleStatChangesEvents(StatChangedEvent ev)
        {
            switch (ev.StatType)
            {
                case BaseStatType.Health:
                    EventBus<DrawDamageEvent>.Raise(new DrawDamageEvent(this, ev.Container.GetCurrent - ev.Container.CachedValue));
                    break;
                case BaseStatType.Stamina:
                    break;
                case BaseStatType.Energy:
                    break;
            }

        }

        public virtual void DisableUnit()
        {
            _stats.StatsUpdatedEvent -= HandleStatChangesEvents;

            _inputs.StopController();
            _inventory.StopController();
            _stats.StopController();
            StopAllCoroutines();
        }


        public virtual void RunUpdate(float delta)
        {
            if (LockUnit) return;
            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            _inputs.UpdateController(delta);
            _stats.UpdateController(delta);
            _inventory.UpdateController(delta);
        }

        #endregion


        #region unit

        protected void OnInputsCreateSkill(SkillProjectileComponent data, BaseUnit source, Transform where)
        {
            SkillRequestFromInputsSuccessEvent?.Invoke(data, source, where);
        }

        #endregion

        protected abstract void HandleDamage(float value);
        protected abstract void HandleDeath();

        public virtual void ApplyEffect(StatsEffect eff)
        {
            _stats.AddEffect(eff);
        }

        public void UnitDodge(BoosterSkillInstanceComponent bs)
        {
            if (LockUnit || _inputs.LockInputs) return;
            Debug.Log($"whoosh!");
        }

        public virtual void TriggerTogglingEvent_UE(float value)
        {    
            if (_inputs == null) return; //in case we have a scene

            bool result = value > 0;
            //_inputs.GetWeaponController.ToggleTriggersOnMelee(result);
        }






        #region trigger events
        public event TriggerEvent UnitTriggerRequestEvent;
        protected void TriggerEventCallback(BaseUnit tg, BaseUnit src, bool ent, StatsEffect cfg)
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