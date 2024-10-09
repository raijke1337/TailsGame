using Arcatech.Units.Behaviour;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    public class CrawlerUnit : NPCUnit
    {

        [Header("NPC Behaviour : Crawler")]


        [SerializeField, Range(1, 20)] float detectionRange = 10f;
        [SerializeField, Range(0, 1)] float skillUseTreschold = 0.2f;
        [SerializeField] List<NestedList<Transform>> patrolPointVariants;



        protected override void SetupBehavior()
        {
            BehaviourPrioritySelector actionsPriority = new BehaviourPrioritySelector("actions list " + GetName);

            Sequence activateSkillSeq = new Sequence("activate skill", 100);
            bool IsLowHP()
            {
                if (_stats.GetStatValue(BaseStatType.Health).GetPercent < skillUseTreschold)
                {
                    return true;
                }
                else
                {
                    activateSkillSeq.Reset();
                    return false;
                }
            }

            Leaf checkHP = new Leaf(new BehaviourCondition(IsLowHP), "is hp low for skill");
            Leaf useSkill = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.MeleeSkill)), "melee skill");

            activateSkillSeq.AddChild(checkHP);
            activateSkillSeq.AddChild(useSkill);

            actionsPriority.AddChild(activateSkillSeq);

            Sequence chasePlayerSeq = new Sequence("chasing player", 50);
            bool PlayerInRange()
            {
                if (Vector3.SqrMagnitude(_player.position - transform.position) < detectionRange) // placeholder
                {
                    return true;
                }
                else
                {
                    chasePlayerSeq.Reset();
                    return false;
                }
            }

            chasePlayerSeq.AddChild(new Leaf(new BehaviourCondition(PlayerInRange), "is player in range"));
            chasePlayerSeq.AddChild(new Leaf(new MoveToPointStrategy(agent, _player), "move to player"));
            chasePlayerSeq.AddChild(new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.Melee)), "use melee attack"));
            chasePlayerSeq.AddChild(new Leaf(new BehaviourAction(() => chasePlayerSeq.Reset()), "reset chase"));

            actionsPriority.AddChild(chasePlayerSeq);

            if (patrolPointVariants!=null && patrolPointVariants.Count > 0)
            {
                BehaviourRandomSelector randomPatrol = new BehaviourRandomSelector("patrol points", 10);
                for (int i = 0; i < patrolPointVariants.Count; i++)
                {
                    Leaf p = new Leaf(new PatrolPointsStrategy(transform, agent, patrolPointVariants[i].InternalList), $"points option {i}");
                    randomPatrol.AddChild(p);
                }

                actionsPriority.AddChild(randomPatrol);
            }
            else
            {
                actionsPriority.AddChild(SetupIdleRoaming());
            }

            tree.AddChild(actionsPriority);
        }
    }

}