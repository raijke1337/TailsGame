using Arcatech.Skills;

namespace Arcatech.Items
{
    public abstract class BaseSkillUsageStrategy : ISkillUseStrategy
    {
        public abstract SkillProjectileComponent Prefab { get; }

        public abstract void SkillUseStateEnter();
        public abstract void SkillUseStateExit();
    }

}