using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "Self effec skill Use Strategy", menuName = "Strategy/Skill/Effect Self")]
    public class SerializedSelfEffectSkillUsageStrategy : SerializedSkillUseStrategy
    {
        public SerializedStatsEffectConfig[] Configs; 
        public override BaseSkillUsageStrategy ProduceStrategy(BaseUnit owner, SerializedSkill cfg, BaseEquippableItemComponent item)
        {
            return new SelfEffectSkillUsageStrategy(Configs, item, OnUseAction, owner, cfg, Charges, Reload, CD);
        }
    }

}