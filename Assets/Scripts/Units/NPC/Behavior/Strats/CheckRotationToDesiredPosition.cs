using UnityEngine;
using UnityEngine.AI;
using static Arcatech.Units.Behaviour.Node;

namespace Arcatech.Units.Behaviour
{
    public class CheckRotationToDesiredPosition : IBehaviorTreeStrategy
    {
        readonly float y;
        NavMeshAgent agent;
        public CheckRotationToDesiredPosition(float frontAngleY, NavMeshAgent agent)
        {
            y = frontAngleY;
            this.agent = agent;
        }

        public NodeStatus Process(ControlledUnit actor)
        {
            Vector3 fwd = actor.transform.forward;
            Vector3 toTarget = (agent.destination - actor.transform.position).normalized;
            Vector3 cross = Vector3.Cross(fwd, toTarget);
            //Debug.Log($"{cross}");
            return NodeStatus.Success;
            /// palceholder

        }

        public void Reset()
        {
        }
    }
}