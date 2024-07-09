using Arcatech.Units;

namespace Arcatech.Skills
{
    public abstract class BaseSkillUsageStrategy : IUseStrategy
    
    {
        protected ISkill Skill;
        public DummyUnit Owner { get;}


        public abstract void SkillUseStateEnter();
        public abstract void SkillUseStateExit();
        public BaseSkillUsageStrategy (DummyUnit owner, ISkill skill)
        {
            Owner = owner;
            Skill = skill;
        }

    }

}