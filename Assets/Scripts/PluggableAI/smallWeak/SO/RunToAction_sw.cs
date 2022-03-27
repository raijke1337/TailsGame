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
[CreateAssetMenu(menuName = "aiAssets/Actions/RunTo")]
public class RunToAction_sw : Action
{
    public override void Act(NPCUnitControllerAI controller)
    {
        MoveTo(controller);
    }

    private void MoveTo(NPCUnitControllerAI controller)
    {
        controller.NavMeshAg.destination = controller.ChaseTarget.position;
        controller.NavMeshAg.Resume();
    }



}

