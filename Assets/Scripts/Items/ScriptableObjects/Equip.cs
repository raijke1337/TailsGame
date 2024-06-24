using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Triggers;
using System;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable, CreateAssetMenu(fileName = "New Equip Item", menuName = "Items/Equip Item")]
    public class Equip : Item
    {

        public BaseEquippableItemComponent Item;
        public SerizalizedSkillConfiguration Skill;
        public SerializedStatTriggerConfig CostTrigger; 
        public SerializedEffectsCollection Effects;
        public RuntimeAnimatorController AnimatorController;

    }
}