using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Scenes.Cameras;
using Arcatech.Texts;
using Arcatech.Units.Inputs;
using KBCore.Refs;
using UnityEngine;


namespace Arcatech.Units
{
    [RequireComponent(typeof(InputsPlayer))]
    public class PlayerUnit : ControlledUnit
    {
        [SerializeField, Child] protected FaceCameraCOntrroller _faceCam;
        public AimingComponent GetAimingComponent => (_inputs as InputsPlayer).Aiming;

        

        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            

            ToggleCamera(true);
        }
        public bool PlayerArmed
        { get
            {
                return _inventory.IsItemEquipped(EquipmentType.MeleeWeap, out _) || _inventory.IsItemEquipped(EquipmentType.RangedWeap, out _);
        } }

        protected override void RaiseStatChangeEvent(StatChangedEvent ev)
        {
            EventBus<StatChangedEvent>.Raise(ev);
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
                return DataManager.Instance.GetCurrentPlayerItems;
            }

        }
        #endregion

        #region animator

        public void ComboWindowStart()
        {
            _animator.SetBool("AdvancingCombo", true);
           // _playerController.IsInMeleeCombo = true;
        }
        public void ComboWindowEnd()
        {
            _animator.SetBool("AdvancingCombo", false);
            //_playerController.IsInMeleeCombo = false;
        }

        private void HandleShieldBreakEvent()
        { if (_inputs.DebugMessage)
                Debug.Log($"Inputs report shield break event");
        }
        public void PlayerIsTalking(DialoguePart d)
        {
            if (_inputs.DebugMessage)
                Debug.Log($"Dialogue window is shown by player panel script, dialogue is: {d.Mood}");
        }

        protected override void HandleDamage(float value)
        {
            base.HandleDamage(value);

            if (_inputs.DebugMessage)
                Debug.Log($"Player unit handle dmg event");
        }

        protected override void HandleDeath()
        {
            _animator.SetTrigger("KnockDown");
            if (_inputs.DebugMessage)
                Debug.Log($"Player unit handle death event");
        }

        public override ReferenceUnitType GetUnitType()
        {
            return ReferenceUnitType.Player;
        }

        public override void AddItem(Item item, bool equip)
        {
            base.AddItem(item,equip);
            _animator.SetTrigger("ItemPickup");
            if (_inputs.DebugMessage)
            {
                Debug.Log($"Player picked up item {item}");
            }
        }
    }

    #endregion
}