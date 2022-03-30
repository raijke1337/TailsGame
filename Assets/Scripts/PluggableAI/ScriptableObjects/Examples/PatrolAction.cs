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
    public override void Act(InputsNPC controller)
    {
        Patrol(controller);
    }

    private void Patrol(InputsNPC controller)
    {
        // go towards point
        controller.NavMeshAg.destination = 
            controller.PatrolPoints[controller.NextPatrolPointIndex].position;
        controller.NavMeshAg.Resume();
    }

}

