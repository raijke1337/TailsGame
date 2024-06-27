using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon")]
    public class Weapon : Equip
    {
        public SerializedStatsEffectConfig Cost;
        public SerializedStatsEffectConfig[] UseEffects;
        public DrawItemsStrategy DrawStrategy;

    }
}