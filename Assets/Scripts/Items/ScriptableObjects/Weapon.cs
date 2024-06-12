using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon")]
    public class Weapon : Equip
    {
        [Tooltip("Time between uses"),Range(0.05f,1f)]public float WeaponCooldown;
        public List<SerializedStatTriggerConfig>  WeaponHitTriggers;
    }
}