using Arcatech.Effects;
using Arcatech.Managers;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Items
{
    [RequireComponent(typeof(WeaponTriggerComponent))]
    public class MeleeWeapon : BaseWeapon
    {

        protected override void FinishWeaponConfig()
        {
            var t = GetComponent<WeaponTriggerComponent>();
            _triggers.Add(t);
            t.TriggerHitUnitEvent  += OnTriggerHit;
        }

        private void OnTriggerHit(BaseUnit target,bool isenter)
        {
            TriggerActivationCallback(target);
        }

        public void ToggleColliders(bool enable)
        {
            _triggers[0].ToggleCollider(enable);
        }
    }

}