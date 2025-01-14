using Arcatech.Triggers;
using Arcatech.Units;
using KBCore.Refs;
using UnityEngine;
namespace Arcatech.Items
{
    [RequireComponent(typeof(WeaponTriggerComponent),typeof(MeleeWeaponTrail))]
    public class MeleeWeaponComponent : BaseWeaponComponent
    {

        public WeaponTriggerComponent Trigger { get; protected set; }
        public MeleeWeaponTrail Trail { get;protected set; }

        private void Awake()
        {
            Trigger = GetComponent<WeaponTriggerComponent>();
            Trail = GetComponent<MeleeWeaponTrail>();
        }
    }

}