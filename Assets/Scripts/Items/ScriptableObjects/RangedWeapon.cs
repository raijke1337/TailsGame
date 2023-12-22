using System;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Ranged Weapon Item", menuName = "Items/Ranged Weapon")]
    public class RangedWeapon : Weapon
    {
        public RangedWeaponConfig RangedConfig;

    }
}