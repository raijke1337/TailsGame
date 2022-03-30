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

[CreateAssetMenu(menuName = "PluggableAI/Actions/DummyDoNothing")]
public class DummyAction : Action
{
    public override void Act(InputsNPC controller)
    {
        // nothing happens
    }
}

