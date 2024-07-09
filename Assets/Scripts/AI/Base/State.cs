using Arcatech.Units.Inputs;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/State"), System.Serializable]
    public class State : ScriptableObject
    {
        // what can be done in the state
        public List<Action> actions;
        // possible evaluations
        public List<Transition> transitions;
        public float StateExpiryTime = 2f;
        //time in seconds for various purposes

        // for gizmos drawing
        public Color StateGizmoColor = Color.gray;

        // state is updated every frame by statecontroler
        public void UpdateState(EnemyStateMachine controller)
        {
            DoActions(controller);
            CheckTransitions(controller);
        }

        private void DoActions(EnemyStateMachine controller)
        {
            foreach (var action in actions)
            {
                action.Act(controller);
            }
        }

        private void CheckTransitions(EnemyStateMachine controller)
        {
            foreach (var tr in transitions)
            {
                if (tr.decision.Decide(controller)) controller.TransitionToState(tr.trueState);
                else controller.TransitionToState(tr.falseState);
            }
        }

    }

}