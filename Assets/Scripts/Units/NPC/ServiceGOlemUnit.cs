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

public class ServiceGolemUnit : NPCUnit
{

    protected override void Start()
    {
        base.Start();
        (_controller as ServiceGolemInputs).SetDodgeCtrl(GetID);
    }
}

