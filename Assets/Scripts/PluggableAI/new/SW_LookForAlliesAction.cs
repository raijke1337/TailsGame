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
public class SW_LookForAlliesAction : Action
{
    public override void Act(NPCUnitControllerAI controller)
    {
        Search(controller);
    }


    private void Search(NPCUnitControllerAI controller)
    {

    }
}

