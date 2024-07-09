// a predicate is a function that tests a condition and then returns a bool

using System;

namespace Arcatech.StateMachine
{
    public class FuncPredicate : IPredicate
    {
        readonly Func<bool> f;

        public FuncPredicate(Func<bool> f)
        {
            this.f = f;
        }

        public bool Evaluate() => f.Invoke();
    }
}

