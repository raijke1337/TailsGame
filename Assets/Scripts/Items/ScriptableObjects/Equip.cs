using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
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