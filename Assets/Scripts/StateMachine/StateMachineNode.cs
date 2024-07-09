

using System.Collections.Generic;

namespace Arcatech.StateMachine
{
    /// <summary>
    /// state and transitions
    /// </summary>
    public class StateMachineNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; } 
        // hashset is an unordered collection of unique elements
        public StateMachineNode (IState s)
        {
            State = s; Transitions = new HashSet<ITransition>();
        }
        public void AddTransition(IState to, IPredicate cond)
        {
            Transitions.Add(new StateTransition(to, cond));
        }

    }

}

