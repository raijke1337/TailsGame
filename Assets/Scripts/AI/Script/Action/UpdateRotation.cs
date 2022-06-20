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

[CreateAssetMenu(menuName = "AIConfig/Action/UpdateRotation")]
public class UpdateRotation : Action
{
    public override void Act(StateMachine controller)
    {
        //if (controller.NMAgent.isStopped == true) controller.NMAgent.isStopped = false;

        controller.OnRotateRequest();
    }
}

