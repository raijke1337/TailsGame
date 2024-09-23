using UnityEngine;
using UnityEngine.AI;
using static Arcatech.Units.Behaviour.Node;

namespace Arcatech.Units.Behaviour
{
    public class MoveToPointStrategy : IBehaviorTreeStrategy
    {
        readonly Transform entity;
        readonly NavMeshAgent nma;
        readonly Transform target;
        bool isPathCalculated;

        public MoveToPointStrategy(Transform entity, NavMeshAgent nma, Transform target)
        {
            this.entity = entity;
            this.nma = nma;
            this.target = target;
        }

        public NodeStatus Process(ControlledUnit actor)
        {
            if (isPathCalculated && nma.remainingDistance < nma.stoppingDistance)
            {
                Reset();
                return NodeStatus.Success;
            }

            nma.SetDestination(target.position);
            entity.LookAt(target.position);

            if (!nma.pathPending)
            {
                isPathCalculated = true;
            }
            return NodeStatus.Running;
        }

        public void Reset() => isPathCalculated = false;
    }

}