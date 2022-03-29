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
[CreateAssetMenu(menuName = "aiAssets/Actions/PatrolTo")]
public class PatrolTo_sw : Action
{
    public override void Act(NPCUnitControllerAI controller)
    {
        Patrol(controller);
    }

    private void Patrol(NPCUnitControllerAI controller)
    {

        // go towards point
        controller.NavMeshAg.destination =
            controller.PatrolPoints[controller.NextPatrolPointIndex].position;
        controller.NavMeshAg.Resume();
        // check if the stopping distance is more than the remaining distance

    }



}

