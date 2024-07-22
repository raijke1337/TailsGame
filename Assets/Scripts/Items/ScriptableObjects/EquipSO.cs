using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Equip Item", menuName = "Items/Equip Item")]
    public class EquipSO : ItemSO
    {
        public BaseEquippableItemComponent ItemPrefab;
        public SerializedStatModConfig[] StatMods;
        public SerializedSkill Skill;
    }
}