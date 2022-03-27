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

[CreateAssetMenu(menuName = "aiAssets/Decisions/Retreat")]
public class RetreatDecision_sw : Decision
{
    public override bool Decide(NPCUnitControllerAI controller)
    {
        return LookForAllies(controller);
    }
    private bool LookForAllies(NPCUnitControllerAI controller)
    {
        return controller.FleeingStateRequest();
    }
}

