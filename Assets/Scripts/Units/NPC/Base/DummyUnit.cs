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


        public UnitInventoryController GetInventoryComponent => _inventory;

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
            base.RunUpdate(delta);

            if (LockUnit) return;
            _stats.ControllerUpdate(delta);
            _inventory.ControllerUpdate(delta);
        }

        public override void RunFixedUpdate(float delta)
        {
            base.RunFixedUpdate(delta);

            _stats.FixedControllerUpdate(delta);
            _inventory.FixedControllerUpdate(delta);

        }


        public override ReferenceUnitType GetUnitType()
        {
            return ReferenceUnitType.Any;
        }



        #region inventory
        // move all event triggers to event bus system
        // that incluides invenotry updates from picking up items/

        //public virtual void AddItem(ItemSO item, bool equip)
        //{
        //    _inventory.OnplayerPickedUpItem(item, equip);
        //}

        protected virtual UnitInventoryItemConfigsContainer SelectSerializedItemsConfig()
        {
            return new UnitInventoryItemConfigsContainer(defaultEquips);
        }

        #endregion

        #region stats

        public virtual void ApplyEffect(StatsEffect eff)
        {
            _stats.AddEffect(eff);
        }
        public event UnityAction<DummyUnit> BaseUnitDiedEvent = delegate { };

        protected virtual void RaiseStatChangeEvent(StatChangedEvent ev)
        {
            switch (ev.StatType)
            {
                case BaseStatType.Health:
                    EventBus<DrawDamageEvent>.Raise(new DrawDamageEvent(this, ev.Container.GetCurrent - ev.Container.CachedValue));
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