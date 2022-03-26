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
[CreateAssetMenu(menuName = "PluggableAI/Actions/SW/FleeAction")]
public class sw_LookForAlliesAction : Action
{
    public override void Act(NPCUnitControllerAI controller)
    {
        RunAway(controller);
    }


    private void RunAway(NPCUnitControllerAI controller)
    {
        Vector3 desired = (controller.ChaseTarget.forward - controller.transform.position)*controller.GetStats[EnemyStatType.LookRange];
        controller.NavMeshAg.destination = desired;
    }
}

