using Arcatech.Items;
using UnityEngine;

namespace Arcatech.Units
{

    public class DummyUnit : BaseEntity
    {
        [Space, SerializeField] protected UnitInventoryController _inventory;
        [SerializeField] protected UnitItemsSO defaultEquips;
        [SerializeField] protected ItemEmpties itemEmpties;
        [SerializeField] protected DrawItemsStrategy defaultItemsDrawStrat;


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



    }

}