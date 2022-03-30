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

[CreateAssetMenu(menuName = "aiAssets/Decisions/DoneIdling")]
public class DoneIdlingDecision_sw : Decision
{
    public override bool Decide(InputsNPC controller)
    {
        return CheckIfDoneIdling(controller);
    }
    private bool CheckIfDoneIdling(InputsNPC controller)
    {
        bool result = controller.IsStateCountdownElapsed(controller.idleTime);

        // set a patrol point here for SOLID
        if (result)
        {
            controller.NextPatrolPointIndex = (controller.NextPatrolPointIndex + 1) % controller.PatrolPoints.Count;
            return true;
        }

        return result;
    }
}

