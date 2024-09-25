using UnityEngine;
using UnityEngine.AI;
using static Arcatech.Units.Behaviour.Node;

namespace Arcatech.Units.Behaviour
{


    public class RoamInRangeStrategy : IBehaviorTreeStrategy
    {
        readonly Vector3 startPos;
        readonly float roamRadius;
        Vector3 currentTarget;
        bool isPathCalculated;
        NavMeshAgent agent;
        bool unset = true;

        public RoamInRangeStrategy(float rad, Vector3 start, NavMeshAgent agent)
        {
            this.roamRadius = rad;
            startPos = start;
            isPathCalculated = false;
            this.agent = agent;
        }
        public NodeStatus Process(ControlledUnit actor)
        {
            if (!unset && isPathCalculated && agent.remainingDistance < agent.stoppingDistance)
            {
                return NodeStatus.Success;
            }

            if (unset)
            {
                var newPoint = Random.insideUnitCircle * roamRadius;
                var newDest = new Vector3 (newPoint.x + startPos.x, startPos.y, startPos.z+newPoint.y);
                if (NavMesh.SamplePosition(newDest,out var h,20f,NavMesh.AllAreas))
                {
                    //Debug.Log($"New destination OK {newDest}");
                    currentTarget = newDest;
                    agent.SetDestination(newDest);
                    unset = false;
                    return NodeStatus.Running;
                }
                else
                {
                    //Debug.Log($"Fail to sample pos {newDest}");
                    return NodeStatus.Fail;
                }

            }
            if (!agent.pathPending)
            {
                isPathCalculated = true;
            }
            return NodeStatus.Running;

        }

        public void Reset()
        {
            unset = true;
            isPathCalculated = false;
        }
    }
}