using System;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Items
{
    [Serializable]
    public abstract class BaseWeapon : BaseEquippableItemComponent
    {

        public bool IsSetup { get; private set; } = false;
        protected StatValueContainer WeaponUses;

        protected float InternalCooldown;
        protected float _currentCooldown;

        public float GetRemainingUses { get { return WeaponUses.GetCurrent; } }

        protected bool IsBusy = false;
        protected List<string> _effectsIDs;

        public virtual bool UseWeapon(out string reason)
        {
            reason = "Ok";

            if (_currentCooldown < InternalCooldown)
            {
                reason = "Weapon on cooldown";
                return false;
            }
            else
            {
                _currentCooldown = 0f;
                return true;
            }
        }

        // loaded by weaponcontroller
        public virtual void AddTriggerData(string effectID)
        {
            if (_effectsIDs == null) _effectsIDs = new List<string>();
            _effectsIDs.Add(effectID);
        }

        public virtual void SetUpWeapon(BaseWeaponConfig config)
        {

            foreach (string triggerID in config.TriggerIDs)
            {
                AddTriggerData(triggerID);
            }
            WeaponUses = new StatValueContainer(config.Charges);
            InternalCooldown = config.InternalCooldown;
            IsSetup = true;
        }

        public override void UpdateInDelta(float deltaTime)
        {
            _currentCooldown += deltaTime;
        }
    }

}