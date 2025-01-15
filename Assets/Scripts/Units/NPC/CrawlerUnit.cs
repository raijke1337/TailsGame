using Arcatech.Units.Behaviour;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    public class CrawlerUnit : NPCUnit
    {

        [Header("NPC Behaviour : Crawler")]

        [SerializeField, Range(0, 1)] float skillUseTreschold = 0.2f;

        protected override void SetupBehavior()
        {
            BehaviourPrioritySelector actionsPriority = new BehaviourPrioritySelector("actions list " + GetName);

            //Sequence activateSkillSeq = new Sequence("activate skill", 100);
            //bool IsLowHP()
            //{
            //    if (_stats.GetStatValue(BaseStatType.Health).GetPercent < skillUseTreschold)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        activateSkillSeq.Reset();
            //        return false;
            //    }
            //}

            //Leaf checkHP = new Leaf(new BehaviourCondition(IsLowHP), "is hp low for skill");
            //Leaf useSkill = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.MeleeSkill)), "melee skill");

            //activateSkillSeq.AddChild(checkHP);
            //activateSkillSeq.AddChild(useSkill);

            //actionsPriority.AddChild(activateSkillSeq);

            //Sequence chasePlayerSeq = new Sequence("chasing player", 50);
            //bool PlayerInRange()
            //{
            //    if (Vector3.SqrMagnitude(_player.position - transform.position) < detectionRange) // placeholder
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        chasePlayerSeq.Reset();
            //        return false;
            //    }
            //}

            //chasePlayerSeq.AddChild(new Leaf(new BehaviourCondition(PlayerInRange), "is player in range"));
            //chasePlayerSeq.AddChild(new Leaf(new MoveToPointStrategy(agent, _player), "move to player"));
            //chasePlayerSeq.AddChild(new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.Melee)), "use melee attack"));
            //chasePlayerSeq.AddChild(new Leaf(new BehaviourAction(() => chasePlayerSeq.Reset()), "reset chase"));

            //actionsPriority.AddChild(chasePlayerSeq);

            Sequence inCombat = new Sequence("combat activity",50);
            Leaf checkCombatState = new Leaf(new BehaviourCondition(() => UnitInCombatState == true), "check combat state");
            Leaf chase = new Leaf(new MoveToTransformStrategy(agent, _player), "move to player");

            inCombat.AddChild(checkCombatState);
            inCombat.AddChild(chase);

            actionsPriority.AddChild(inCombat);


            if (patrolPointVariants != null && patrolPointVariants.Count > 0)
            {
                actionsPriority.AddChild(SetupPatrolPoints());
            }
            actionsPriority.AddChild(SetupIdleRoaming());

            tree.AddChild(actionsPriority);
        }
    }

}