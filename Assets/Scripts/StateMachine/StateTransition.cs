namespace Arcatech.StateMachine
{
    public class StateTransition : ITransition
    {
        public StateTransition(IState state, IPredicate pred)
        {
            NextState = state;
            Condition = pred;
        }

        public IState NextState { get; }
        public IPredicate Condition { get; }
    }

}

