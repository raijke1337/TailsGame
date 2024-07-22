using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Skills
{
    [CreateAssetMenu(fileName = "Shoot projectile skill Use Strategy", menuName = "Strategy/Skill/Projectile")]
    public class SerializedProjectileSkillUsageStrategy: SerializedSkillUseStrategy
    {
        public SerializedProjectileConfiguration Projectile;
        public SerializedStatsEffectConfig[] Effects; 
        public override BaseSkillUsageStrategy ProduceStrategy(BaseUnit owner, SerializedSkill cfg, BaseEquippableItemComponent item)
        {
            return new ProjectileSkillUsageStrategy(Projectile, Effects,item , OnUseAction, owner, cfg, Charges, Reload, CD);
        }
    }

}