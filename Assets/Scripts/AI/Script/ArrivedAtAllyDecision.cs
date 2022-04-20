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
[CreateAssetMenu(menuName = "AIConfig/Decision/ArrivedAtAlly")]
public class ArrivedAtAllyDecision : Decision
{
    public override bool Decide(StateMachine controller)
    {
        var result = Vector3.Distance(controller.NMAgent.transform.position, controller.FoundAlly.transform.position) < controller.NMAgent.stoppingDistance;
        return result;
    }
}

