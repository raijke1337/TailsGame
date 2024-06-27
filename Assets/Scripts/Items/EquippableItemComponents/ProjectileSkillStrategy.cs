using Arcatech.Skills;

namespace Arcatech.Items
{
    public class ProjectileSkillStrategy : BaseSkillUsageStrategy
    {
        public ProjectileSkillStrategy(ProjectileComponent prefab)
        {
            Prefab = prefab as SkillProjectileComponent; // TODO
        }

        public override SkillProjectileComponent Prefab { get; }

        public override void SkillUseStateEnter()
        {
            
        }

        public override void SkillUseStateExit()
        {
            //
        }
    }

}