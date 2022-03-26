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

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(NPCUnitControllerAI controller)
    {
        Patrol(controller);
    }

    private void Patrol(NPCUnitControllerAI controller)
    {
        // go towards point
        controller.NavMeshAg.destination = 
            controller.PatrolPoints[controller.NextPatrolPoint].position;
        controller.NavMeshAg.Resume();
        // check if the stopping distance is more than the remaining distance
        if (controller.NavMeshAg.remainingDistance <= controller.NavMeshAg.stoppingDistance &&
            !controller.NavMeshAg.pathPending)
        {
            // we have arrived, go to next - make sure we dont exceed
            controller.NextPatrolPoint = (controller.NextPatrolPoint + 1) % controller.PatrolPoints.Count;
        }

    }

}

