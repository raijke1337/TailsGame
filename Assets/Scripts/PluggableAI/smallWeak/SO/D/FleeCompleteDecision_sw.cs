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

[CreateAssetMenu(menuName = "aiAssets/Decisions/FleeComplete")]
public class FleeCompleteDecision_sw : Decision
{
    public override bool Decide(NPCUnitControllerAI controller)
    {
        return FleeingExpiryCheck(controller);
    }
    private bool FleeingExpiryCheck(NPCUnitControllerAI controller)
    {
        var distance = Vector3.Distance(controller.transform.position, controller.ChaseTarget.position);
        bool check = distance < controller.NavMeshAg.stoppingDistance;
        if (!check) check = controller.IsStateCountdownElapsed(Constants.Combat.c_FleeTimeout);
        return check;
    }
}

