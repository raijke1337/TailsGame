using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Items
{
    [RequireComponent(typeof(WeaponTriggerComponent))]
    public class MeleeWeaponComponent : BaseWeaponComponent
    {

        protected override void FinishWeaponConfig()
        {
            var t = GetComponent<WeaponTriggerComponent>();
            _triggers.Add(t);
            t.TriggerHitUnitEvent += TriggerActivationCallback;
        }

        public void ToggleColliders(bool enable)
        {
            _triggers[0].ToggleCollider(enable);
        }


    }

}