using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Texts;
using Arcatech.Units.Inputs;
using System.Collections;
using System.Linq;
using UnityEngine;
using KBCore.Refs;
using Arcatech.Scenes.Cameras;


namespace Arcatech.Units
{
    public class PlayerUnit : BaseUnit
    {
        [SerializeField, Child] protected FaceCameraCOntrroller _faceCam;

        public AimingComponent GetAimingComponent => (_inputs as InputsPlayer).Aiming;

        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }

        public override void InitiateUnit()
        {
            base.InitiateUnit();
            ToggleCamera(true);
        }
        public bool PlayerArmed
        { get
            {
                return _inventory.IsItemEquipped(EquipmentType.MeleeWeap, out _) || _inventory.IsItemEquipped(EquipmentType.RangedWeap, out _);
        } }


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
            _animator.SetTrigger("TakeDamage");

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

        protected override void OnItemAdd(Item i)
        {
            _animator.SetTrigger("ItemPickup");
            if (_inputs.DebugMessage)
            {
                Debug.Log($"Player picked up item {i}");
            }
        }
    }

    #endregion
}