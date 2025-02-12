﻿using UnityEngine;
using UnityEngine.AI;
using static Arcatech.Units.Behaviour.Node;

namespace Arcatech.Units.Behaviour
{
    public class AimAtTransform : IBehaviorTreeStrategy
    {
        readonly float angle;
        readonly NavMeshAgent agent;
        readonly Transform aimAt;
        public AimAtTransform (NavMeshAgent agent, Transform point, float angleTolerance)
        {
            this.angle = angleTolerance;
            this.agent = agent;
            this.aimAt = point;
        }

        public NodeStatus Process(ControlledUnit actor)
        {
            Vector3 desired = (aimAt.position - actor.transform.position).normalized;
            desired.y = 0f;
            float currentangle = Vector3.Angle(agent.transform.forward.normalized,desired);   

            if (currentangle < angle)
            {
                agent.isStopped = false;
                return NodeStatus.Success;
            }
            else
            {
                agent.isStopped = true;
                actor.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(actor.transform.forward, desired, 0.1f, 1f));
                return NodeStatus.Running;
            }
        }

        public void Reset()
        {
        }
    }

}