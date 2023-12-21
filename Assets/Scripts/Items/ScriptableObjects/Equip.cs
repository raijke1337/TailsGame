using Arcatech.Effects;
using Arcatech.Skills;
using System;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Equip Item", menuName = "Items/Equip Item")]
    public class Equip : Item
    {

        public SkillControlSettingsSO Skill;
        public BaseEquippableItemComponent Item;
        public EffectsCollection Effects;

    }
}