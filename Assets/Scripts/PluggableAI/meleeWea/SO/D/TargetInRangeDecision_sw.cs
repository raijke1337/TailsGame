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
[CreateAssetMenu(menuName = "aiAssets/Decisions/TgtInRange")]
public class TargetInRangeDecision_sw : Decision
{
    public override bool Decide(NPCUnitControllerAI controller)
    {
        return CheckAttackRange(controller);
    }

    private bool CheckAttackRange(NPCUnitControllerAI controller)
    {
        float currentDist = Vector3.Distance(controller.transform.position, controller.ChaseTarget.position);
        return currentDist < controller.GetStats.AttackRange;
    }

}

