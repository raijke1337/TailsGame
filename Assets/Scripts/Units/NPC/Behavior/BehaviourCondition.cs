using System;

namespace Arcatech.Units.Behaviour
{
    /// <summary>
    /// logical IF
    /// </summary>
    public class BehaviourCondition : IBehaviorTreeStrategy
    {
        readonly Func<bool> predicate;

        public BehaviourCondition(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public Node.NodeStatus Process(ControlledUnit actor)
        {
            if (predicate()) return Node.NodeStatus.Success;
            else return Node.NodeStatus.Fail;
        }

        public void Reset()
        {
        }
    }

}