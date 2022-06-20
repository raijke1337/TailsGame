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
[CreateAssetMenu(menuName = "AIConfig/Decision/IsPlayerInSphereCast")]
public class PlayerInFrontDecision : Decision
{
    public override bool Decide(StateMachine controller)
    {
        return controller.OnLookSphereCast();
    }
}

