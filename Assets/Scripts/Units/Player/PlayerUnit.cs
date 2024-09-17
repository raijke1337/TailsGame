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
        protected override void UpdateStats()
        {
            base.UpdateStats();
            foreach (var pair in _stats.GetStatValues)
            {
                EventBus<PlayerStatsChangedUIEvent>.Raise(new PlayerStatsChangedUIEvent(pair.Key,pair.Value));
            }
             // used by player UI
        }
        #endregion




    }

}