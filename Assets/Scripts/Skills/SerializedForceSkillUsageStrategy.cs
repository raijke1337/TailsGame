using Arcatech.Items;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "Apply force skill Use Strategy", menuName = "Strategy/Skill/Force")]
    public class SerializedForceSkillUsageStrategy : SerializedSkillUseStrategy
    {
        public Vector3 Force;
        public override BaseSkillUsageStrategy ProduceStrategy(BaseUnit owner, SerializedSkill cfg, BaseEquippableItemComponent item)
        {
            return new ApplyForceSkillUsageStrategy(Force, item, OnUseAction, owner, cfg,Charges,Reload,CD);
        }
    }

}