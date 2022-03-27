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

[CreateAssetMenu(menuName = "aiAssets/Decisions/HPCheck")]
public class HPdecision_sw : Decision
{
    public override bool Decide(NPCUnitControllerAI controller)
    {
        return CheckHP(controller);
    }
    private bool CheckHP(NPCUnitControllerAI controller)
    {
        return (controller.GetUnit.GetStats()[StatType.Health].GetCurrent() >= controller.GetStats[EnemyStatType.FleeHealth]);
    }
}

