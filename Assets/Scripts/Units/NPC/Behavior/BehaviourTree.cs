using System;
using UnityEngine;

namespace Arcatech.Units.Behaviour
{
    /// <summary>
    /// behavior tree is a sequence of behaviors to execute
    /// </summary>

    public class BehaviourTree : Node
    {

        public BehaviourTree(string n, int p = 0) : base (n,p)
        {

        }
        public override NodeStatus Process(NPCUnit actor)
        {
            NodeStatus status = Children[currentChildIndex].Process(actor);



            if (status == NodeStatus.Success)
            {
                return status;
            }
            currentChildIndex = (currentChildIndex + 1) % Children.Count;

            return NodeStatus.Running;
        }
    }
}