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
[CreateAssetMenu(menuName = "AIConfig/Decision/SupportAlly")]
public class SwitchToSupportDecision : Decision
{
    public override bool Decide(StateMachine controller)
    {
        var stats = controller.SelectedUnit.GetStats()[BaseStatType.Health];
        return stats.GetCurrent / stats.GetMax <= 0.5; // todo , now heals at 50% hp left 50/100 = 0.5
    }

}

