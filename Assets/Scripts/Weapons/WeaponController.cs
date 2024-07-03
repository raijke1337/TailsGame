using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Stats;
using Arcatech.Triggers;
using KBCore.Refs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class WeaponController : ManagedControllerBase, ICombatActions
    {
        protected Dictionary<UnitActionType, IWeapon> _weapons;
        protected UnitStatsController _stats;

        private UnitInventoryController _inv;

        private EventBinding<InventoryUpdateEvent> bind;


        public WeaponController(UnitStatsController stats, UnitInventoryController inv, DummyUnit dummyUnit) : base(dummyUnit)
        {
            _stats = stats;
            _inv = inv;
            bind = new EventBinding<InventoryUpdateEvent>(OnInventoryUpdate);
            UpdateWeapons(_inv);
    
        }

        private void OnInventoryUpdate(InventoryUpdateEvent obj)
        {
            UpdateWeapons(_inv);
        }

        void UpdateWeapons(UnitInventoryController i)
        {
            _weapons = new();
            foreach (var weapon in i.GetWeapons)
            {
                _weapons[weapon.UseActionType] = weapon;
                _weapons[weapon.UseActionType].AssignStrategy();
            }
        }


        #region interface
        public bool TryUseAction(UnitActionType action)
        {
            if (_weapons.ContainsKey(action))
            {
                var cost =  _weapons[action].GetCost;
                if (cost == null || _stats.TryApplyCost(cost))
                {
                    _inv.DrawItems(_weapons[action].DrawStrategy);
                    _weapons[action].UseItem();
                    return true;
                }
            }
            return false;
        }


        #endregion


        #region managed
        public override void StartController()
        {
            EventBus<InventoryUpdateEvent>.Register(bind);
        }
        public override void ControllerUpdate(float delta)
        {

        }

        public override void FixedControllerUpdate(float fixedDelta)
        {

        }

        public override void StopController()
        {
            EventBus<InventoryUpdateEvent>.Deregister(bind);
        }

        #endregion
    }

}