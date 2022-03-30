using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public class State : ScriptableObject
{
    // what can be done in the state
    public Action[] actions;
    // possible evaluations
    public Transition[] transitions;

    // for gizmos drawing
    public Color StateGizmoColor = Color.gray;

    // state is updated every frame by statecontroler
    public void UpdateState(InputsNPC controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(InputsNPC controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions(InputsNPC controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSuccess = transitions[i].decision.Decide(controller);
            if (decisionSuccess)
            {
                controller.TransitionToState(transitions[i].trueState);
            }
            if (!decisionSuccess)
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }

}

