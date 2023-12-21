using Arcatech.Effects;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Items
{
    [Serializable]
    public abstract class BaseWeapon : BaseEquippableItemComponent
    {
        protected StatValueContainer WeaponUses;

        protected float InternalCooldown;
        protected float _currentCooldown;


        protected bool IsBusy = false;

        #region triggers

        protected List<WeaponTriggerComponent> _triggers;
        protected List<BaseStatTriggerConfig> _weaponEffects;

        public event TriggerEvent TriggerEvent;

        protected void TriggerActivationCallback(BaseUnit target)
        {
            foreach (var eff in _weaponEffects)
            {
                TriggerEvent?.Invoke(target, Owner,true,eff);
            }
        }

        #endregion
        public virtual bool UseWeapon()
        {

            if (_currentCooldown < InternalCooldown)
            {
                return false;
            }
            else
            {
                _currentCooldown = 0f;
                return true;
            }
        }

        public void SetUpWeapon(BaseWeaponConfig config)
        {
            if (_weaponEffects == null) _weaponEffects = new List<BaseStatTriggerConfig>();
            _weaponEffects.AddRange(config.WeaponEffects);

            WeaponUses = new StatValueContainer(config.Charges);
            InternalCooldown = config.InternalCooldown;
            _triggers = new List<WeaponTriggerComponent>();
            FinishWeaponConfig();
        }
        protected abstract void FinishWeaponConfig();


        public override void UpdateInDelta(float deltaTime)
        {
            _currentCooldown += deltaTime;
        }
        
    }

}