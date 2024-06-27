using Arcatech.Effects;
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
        UnitStatsController _stats;

        private UnitInventoryComponent _inv;

        #region interface
        public bool TryUseAction(UnitActionType action)
        {
            if (_weapons.ContainsKey(action))
            {
                var cost =  _weapons[action].GetCost;
                if (_stats.TryApplyCost(cost))
                {
                    _inv.DrawItems(_weapons[action].DrawStrategy);
                    _weapons[action].UseItem();
                    return true;
                }
            }
            return false;
        }

        public IUsesStats SetStats(UnitStatsController s)
        {
            _stats = s;
            return this;
        }

        #endregion

        public WeaponController UpdateWeapons(IWeapon[] all)
        {
            _weapons = new();
            foreach (var weapon in all)
            {
                _weapons[weapon.UseActionType] = weapon;
                _weapons[weapon.UseActionType].AssignStrategy();
            }

            return this;
        }


        #region managed
        public override void StartController()
        {

        }
        public override void ControllerUpdate(float delta)
        {

        }

        public override void FixedControllerUpdate(float fixedDelta)
        {

        }

        public override void StopController()
        {

        }

        public IUsesStats SetInventory(UnitInventoryComponent comp)
        {
            _inv = comp;
            return this;
        }

        #endregion
    }

}