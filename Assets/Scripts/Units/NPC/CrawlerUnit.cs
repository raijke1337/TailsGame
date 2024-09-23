using Arcatech.Managers;
using Arcatech.Units.Behaviour;
using KBCore.Refs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Arcatech.Units
{
    public class CrawlerUnit : NPCUnit
    {

        [Header("NPC Behaviour : Crawler")]



        [SerializeField] List<Transform> patrolPoints1;
        [SerializeField] List<Transform> patrolPoints2;

        [SerializeField, Range(1, 20)] float detectionRange = 10f;
        [SerializeField, Range(0, 1)] float skillUseTreschold = 0.2f;


        Transform _player;

        protected override void SetupBehavior()
        {
            base.SetupBehavior();

            _player = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit.transform;
            tree = new BehaviourTree(GetUnitName + " behaviour");

            BehaviourPrioritySelector actionsPriority = new BehaviourPrioritySelector("actions list " + GetUnitName);


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
            chasePlayerSeq.AddChild(new Leaf(new MoveToPointStrategy(transform, agent, _player), "move to player"));
            chasePlayerSeq.AddChild(new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.Melee)), "use melee attack"));

            actionsPriority.AddChild(chasePlayerSeq);

            BehaviourRandomSelector randomPatrol = new BehaviourRandomSelector("patrol points", 10);
            Leaf patrol1 = new Leaf(new PatrolPointsStrategy(transform, agent, patrolPoints1), "points 1");
            Leaf patrol2 = new Leaf(new PatrolPointsStrategy(transform, agent, patrolPoints2), "points 2");

            randomPatrol.AddChild(patrol2);
            randomPatrol.AddChild(patrol1);

            actionsPriority.AddChild(randomPatrol);


            tree.AddChild(actionsPriority);

        }


    }

}