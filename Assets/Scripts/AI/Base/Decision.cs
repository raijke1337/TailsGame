using UnityEngine;
namespace Arcatech.AI
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(EnemyStateMachine controller);
        // gives a response to a state logic dilemma
    }

}