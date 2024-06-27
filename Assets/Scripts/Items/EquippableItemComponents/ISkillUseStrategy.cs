using Arcatech.Skills;
using Arcatech.Units;

namespace Arcatech.Items
{
    public interface ISkillUseStrategy : IStrategy
    {
        SkillProjectileComponent Prefab { get; }
        void SkillUseStateEnter();
        void SkillUseStateExit();
    }

}