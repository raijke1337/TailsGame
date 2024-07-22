using Arcatech.Effects;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon Item")]
    public class WeaponSO : EquipSO
    {
        [Header("Use settings")]
        public SerializedWeaponUseStrategy WeaponUseStrategy;
        public SerializedStatsEffectConfig Cost;

        [Space]
        public SerializedStatsEffectConfig[] UseEffects;
        public DrawItemsStrategy DrawStrategy;
        public SerializedEffectsCollection Effects;

    }
}