using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Units.Inputs;
using KBCore.Refs;
using UnityEngine;


namespace Arcatech.Units
{
    [RequireComponent(typeof(InputsPlayer))]
    public class PlayerUnit : ControlledUnit
    {
        public override ReferenceUnitType GetUnitType() => ReferenceUnitType.Player;

        [SerializeField, Child] protected Camera _faceCam;
        public AimingComponent GetAimingComponent => (_inputs as InputsPlayer).Aiming;


        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            ToggleCamera(true);
        }



        #region inventory

        protected override UnitInventoryItemConfigsContainer SelectSerializedItemsConfig()
        {

            if (DataManager.Instance.IsNewGame)
            {
                return new UnitInventoryItemConfigsContainer(defaultEquips);
            }
            else
            {
                return DataManager.Instance.GetPlayerSaveEquips;
            }

        }
        #endregion

        #region stats
        protected override void RaiseStatChangeEvent(StatChangedEvent ev)
        {
            EventBus<StatChangedEvent>.Raise(ev);
            base.RaiseStatChangeEvent(ev);
        }
        #endregion




    }

}