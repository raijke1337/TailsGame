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
[CreateAssetMenu(menuName = "aiAssets/Decisions/ArrivedAtDestination")]
public class ArrivedAtPointDecision_sw : Decision
{
    public override bool Decide(InputsNPC controller)
    {
        return Arrived(controller);
    }

    private bool Arrived(InputsNPC controller)
    {
        // check if the stopping distance is more than the remaining distance
        return (controller.NavMeshAg.remainingDistance <= controller.NavMeshAg.stoppingDistance &&
            !controller.NavMeshAg.pathPending);

    }
}

