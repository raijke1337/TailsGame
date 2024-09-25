using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Arcatech.Units.Behaviour.Node;
using static Unity.Burst.Intrinsics.X86;

namespace Arcatech.Units.Behaviour
{

    public class PatrolPointsStrategy : IBehaviorTreeStrategy
    {
        readonly Transform entity;
        readonly NavMeshAgent agent;
        readonly List<Transform> patrolPoints;
        readonly float patrolSpeed;
        int currentIndex;
        bool isPathCalculated;

        public PatrolPointsStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
        {
            this.entity = entity;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
            currentIndex = 0;
        }

        public NodeStatus Process(ControlledUnit actor)
        {
            if (currentIndex == patrolPoints.Count)
            { 
                Reset();
                return NodeStatus.Success; 
            }

            var target = patrolPoints[currentIndex];
            agent.SetDestination(target.position);
            entity.LookAt(target.position);

            if (!agent.pathPending)
            {
                isPathCalculated = true;
            }

            if (isPathCalculated && agent.remainingDistance < 1f)
            {
                currentIndex++;
                isPathCalculated = false;
            }



            return NodeStatus.Running;
        }

        public void Reset()
        {
             currentIndex = 0;
            isPathCalculated = false;
        }
    }
}