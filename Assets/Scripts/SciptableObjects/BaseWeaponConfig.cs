using Arcatech.Triggers;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "New BaseWeaponConfig", menuName = "Configurations/Weapons", order = 1)]
    public class BaseWeaponConfig : ScriptableObjectID
    {
        public StatValueContainer Charges;
        public List<BaseStatTriggerConfig> WeaponEffects;

        public float InternalCooldown;

    }

}