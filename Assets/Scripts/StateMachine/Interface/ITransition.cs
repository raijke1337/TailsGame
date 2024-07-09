// a predicate is a function that tests a condition and then returns a bool

namespace Arcatech.StateMachine
{
    public interface ITransition
    {
        IState NextState { get; }
        IPredicate Condition { get; }
    }

}

