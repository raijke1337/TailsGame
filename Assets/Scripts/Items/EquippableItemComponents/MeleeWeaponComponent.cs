using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Items
{
    [RequireComponent(typeof(WeaponTriggerComponent))]
    public class MeleeWeaponComponent : BaseWeaponComponent
    {

        public WeaponTriggerComponent Trigger { get; protected set; }
        private void Awake()
        {
            Trigger = GetComponent<WeaponTriggerComponent>();
        }
    }

}