using Arcatech.Items;
using Arcatech.Triggers;
using System.Linq;
using UnityEngine;

namespace Arcatech.Units
{

    public class DummyUnit : BaseEntity
    {
        [Space,Header("Dummy Unit"), SerializeField] protected UnitItemsSO defaultEquips;
        [SerializeField] protected ItemEmpties itemEmpties;
        [SerializeField] protected DrawItemsStrategy defaultItemsDrawStrat;

        protected UnitInventoryController _inventory;

        public override void StartControllerUnit()
        {

            base.StartControllerUnit();


            _inventory = new UnitInventoryController(SelectSerializedItemsConfig(), itemEmpties, this);
            _inventory.DrawItems(defaultItemsDrawStrat)
                .StartController();

            _stats.AddMods(_inventory.GetCurrentMods);


        }

        public override void DisableUnit()
        {
            base.DisableUnit();
            LockUnit = true;
            _inventory.StopController();
            _stats.StopController();

        }


        public override void RunUpdate(float delta)
        {
            if (LockUnit) return;
            base.RunUpdate(delta);
            _inventory.ControllerUpdate(delta);
        }

        public override void RunFixedUpdate(float delta)
        {
            if (LockUnit) return;
            base.RunFixedUpdate(delta);
            _inventory.FixedControllerUpdate(delta);
        }

        #region lockUnit
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
                OnLockUnit(_locked);
            }
        }
        protected virtual void OnLockUnit(bool locked)
        {
            //Debug.Log($"lock unit {name} {locked}");
        }

        #endregion

        #region inventory

        protected virtual UnitInventoryItemConfigsContainer SelectSerializedItemsConfig()
        {
            return new UnitInventoryItemConfigsContainer(defaultEquips);
        }

        #endregion

/// <summary>
/// shield check
/// </summary>
/// <param name="eff"></param>
        public override void ApplyEffect(StatsEffect eff)
        {
            var shield = _inventory.GetAllEquips.FirstOrDefault(t => t.Type == EquipmentType.Shield);
            if (shield != null)
            {
                Debug.Log($"Unit has a shield, damage reduction NYI");
            }

            base.ApplyEffect(eff);
        }


    }

}