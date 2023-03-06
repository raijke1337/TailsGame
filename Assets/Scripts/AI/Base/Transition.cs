using System;
[Serializable]
public sealed class Transition
{
    // what we evaluate
    public Decision decision;
    public State trueState;
    public State falseState;
}

