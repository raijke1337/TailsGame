using Arcatech.Skills;
using Arcatech.Units;

namespace Arcatech
{
    public interface IUseStrategy : IStrategy
    {
        void SkillUseStateEnter();
        void SkillUseStateExit();
    }

}