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

[CreateAssetMenu(menuName = "aiAssets/Actions/Attack")]
public class AttackAct_sw : Action
{  
    public override void Act(NPCUnitControllerAI controller)
    {
        DoAttacking(controller);
    }

    private void DoAttacking(NPCUnitControllerAI controller)
    {
        controller.AttackRequest();
    }



}


