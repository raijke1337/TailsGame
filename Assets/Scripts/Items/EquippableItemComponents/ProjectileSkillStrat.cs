using Arcatech.EventBus;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Skills
{
    public class ProjectileSkillStrat : BaseSkillUsageStrategy
    {/// <summary>
    /// unused for now
    /// </summary>
    /// <param name="spawn"></param>
    /// <param name="owner"></param>
    /// <param name="skill"></param>
        public ProjectileSkillStrat(Transform spawn, DummyUnit owner, ISkill skill) : base(owner, skill)
        {
            this.spawn = spawn;
            this.skill = skill;
        }
        readonly Transform spawn;
        readonly ISkill skill;
        public override void SkillUseStateEnter()
        {
        }

        public override void SkillUseStateExit()
        {

        }



    }

}