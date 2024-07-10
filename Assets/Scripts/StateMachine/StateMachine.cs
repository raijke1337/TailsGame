using System;
using System.Collections.Generic;

namespace Arcatech.StateMachine
{
    public class ArcaStateMachine
    {
        StateMachineNode currentState;
        Dictionary<Type, StateMachineNode> nodes = new Dictionary<Type, StateMachineNode>();
        HashSet<ITransition> fromAnyState = new HashSet<ITransition>();

        #region public
        public void Update(float d)
        {
            ITransition tr = GetTransition(); // is there any transition from current state?
            if (tr != null)
            {
                ChangeState(tr.NextState);
            }
            currentState.State?.Update(d);
        }
        public void FixedUpdate(float d)
        {
            currentState.State?.FixedUpdate(d);
        }
        public void ForceState(IState state)
        {
            currentState = GetOrAddNode(state);
            currentState.State?.OnEnterState();
        }


        public void AddTransition(IState from, IState to, IPredicate cond)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, cond);
        }
        public void AddAnyTransition(IState to, IPredicate cond)
        {
            fromAnyState.Add(new StateTransition(GetOrAddNode(to).State, cond));
        }
        #endregion

        ITransition GetTransition()
        {
            foreach (var s in fromAnyState)
            {
                if (s.Condition.Evaluate()) return s;
            }
            foreach (var s in currentState.Transitions)
            {
                if (s.Condition.Evaluate()) return s;
            }
            return null;
        }
        void ChangeState(IState to)
        {
            if (to == currentState.State) return;
            else
            {
                var p = currentState.State;
                p?.OnLeaveState();
                to?.OnEnterState();

                currentState = nodes[to.GetType()];
            }
        }
        StateMachineNode GetOrAddNode(IState state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new StateMachineNode(state);
                nodes.Add(state.GetType(), node);
            }
            return node;
        }


    }

}

