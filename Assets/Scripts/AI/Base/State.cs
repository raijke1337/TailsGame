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

[CreateAssetMenu(menuName = "AIConfig/State"),Serializable]
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
    public void UpdateState(StateMachine controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateMachine controller)
    {
        foreach (var action in actions)
        {
            action.Act(controller);
        }
    }

    private void CheckTransitions(StateMachine controller)
    {
        foreach (var tr in transitions)
        {
            if (tr.decision.Decide(controller)) controller.TransitionToState(tr.trueState);
            else controller.TransitionToState(tr.falseState);
        }
    }

}

