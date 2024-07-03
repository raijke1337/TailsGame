using Arcatech.Triggers;
using Arcatech.Units;
using KBCore.Refs;
using UnityEngine;
namespace Arcatech.Items
{
    public class MeleeWeaponComponent : BaseWeaponComponent
    {

        [SerializeField,Child] public WeaponTriggerComponent Trigger;
    }

}