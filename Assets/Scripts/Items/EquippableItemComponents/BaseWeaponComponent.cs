using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
namespace Arcatech.Items
{
    [Serializable]
    public abstract class BaseWeaponComponent : BaseEquippableItemComponent
    {


        protected bool IsBusy = false;

        #region triggers

        protected List<WeaponTriggerComponent> _triggers;
        public event SimpleTriggerEvent PrefabCollisionEvent;
        protected void TriggerActivationCallback(BaseUnit target,bool isEnter)
        {
            PrefabCollisionEvent?.Invoke(target, isEnter);
        }

        #endregion
        private void Start()
        {
            _triggers = new List<WeaponTriggerComponent>();
            FinishWeaponConfig();
        }
        /// <summary>
        /// find triggers here etc
        /// </summary>
        protected abstract void FinishWeaponConfig();



    }

}