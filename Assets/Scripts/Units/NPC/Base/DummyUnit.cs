using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units.Stats;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{

    public class DummyUnit : BaseUnit
    {
        [Space, SerializeField] protected UnitInventoryController _inventory;
        [SerializeField] protected UnitItemsSO defaultEquips;
        [SerializeField] protected ItemEmpties itemEmpties;
        [SerializeField] protected DrawItemsStrategy defaultItemsDrawStrat;

        [Space, SerializeField,] protected UnitStatsController _stats;
        [SerializeField] protected BaseStatsConfig defaultStats;
        public override string GetUnitName { get; protected set; }

        public override void StartControllerUnit()
        {

            base.StartControllerUnit();

            GetUnitName = defaultStats.DisplayName;

            _inventory = new UnitInventoryController(SelectSerializedItemsConfig(), itemEmpties, this);
            _inventory.DrawItems(defaultItemsDrawStrat)
                .StartController();

            _stats = new UnitStatsController(defaultStats.InitialStats, this);

            _stats.AddMods(_inventory.GetCurrentMods)
                .StartController();

            _stats.StatsUpdatedEvent += RaiseStatChangeEvent;

        }

        public override void DisableUnit()
        {
            base.DisableUnit();
            _inventory.StopController();
            _stats.StopController();

        }


        public override void RunUpdate(float delta)
        {
            if (LockUnit) return;
            _stats.ControllerUpdate(delta);
            _inventory.ControllerUpdate(delta);
        }

        public override void RunFixedUpdate(float delta)
        {
            _stats.FixedControllerUpdate(delta);
            _inventory.FixedControllerUpdate(delta);

        }


        public override ReferenceUnitType GetUnitType()
        {
            return ReferenceUnitType.Any;
        }



        #region inventory

        protected virtual UnitInventoryItemConfigsContainer SelectSerializedItemsConfig()
        {
            return new UnitInventoryItemConfigsContainer(defaultEquips);
        }
        public Transform GetSkillTransform (UnitActionType action)
        {
            switch (action)
            {
                case UnitActionType.DodgeSkill:
                    return itemEmpties.ItemPositions[ItemPlaceType.BoosterEmpty];
                case UnitActionType.MeleeSkill:
                    return itemEmpties.ItemPositions[ItemPlaceType.MeleeEmpty];
                case UnitActionType.RangedSkill:
                    return itemEmpties.ItemPositions[ItemPlaceType.RangedEmpty];
                case UnitActionType.ShieldSkill:
                    return itemEmpties.ItemPositions[ItemPlaceType.ShieldEmpty];
                default:
                    return null;
            }
        }

        #endregion

        #region stats

        public virtual void ApplyEffect(StatsEffect eff)
        {
            _stats.TryAddEffect(eff);
        }
        public event UnityAction<DummyUnit> BaseUnitDiedEvent = delegate { };

        protected virtual void RaiseStatChangeEvent(StatChangedEvent ev)
        {
            switch (ev.StatType)
            {
                case BaseStatType.Health:
                    if (ev.Container.GetCurrent < ev.Container.CachedValue)
                    {
                        EventBus<DrawDamageEvent>.Raise(new DrawDamageEvent(this, ev.Container.GetCurrent - ev.Container.CachedValue));
                    }                    
                    if (ev.Container.GetCurrent <= 0f)
                    {
                        BaseUnitDiedEvent.Invoke(this);
                    }
                    break;
                case BaseStatType.Stamina:
                    break;
                case BaseStatType.Energy:
                    break;
            }

        }

        protected virtual void HandleDamage(float value)
        {
            _animator.SetTrigger("TakeDamage");
        }

        protected virtual void HandleDeath()
        {
            _animator.SetTrigger("Death");
        }


        #endregion

    }

}