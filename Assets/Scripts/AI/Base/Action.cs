using UnityEngine;

public abstract class Action : ScriptableObject
{
    // actions are done based on decisions
    public abstract void Act(StateMachine controller);

}

