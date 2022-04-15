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
[CreateAssetMenu(menuName = "AIConfig/Action/MoveTo")]
public class MoveToPointAction : Action
{
    public override void Act(StateMachine controller)
    {
        controller.NMAgent.isStopped = false;
        if (controller.InCombat)
        {
            controller.NMAgent.SetDestination(controller.ChaseUnit.transform.position);
        }
        else
        {
            controller.NMAgent.SetDestination(controller.PatrolPoints[controller.CurrentPatrolPointIndex].position);
        } 
            
    }
}

