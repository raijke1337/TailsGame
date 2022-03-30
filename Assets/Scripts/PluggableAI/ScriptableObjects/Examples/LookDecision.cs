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
[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(InputsNPC controller)
    {
        bool TargetVisible = Look(controller);
        return TargetVisible;
    }

    private bool Look(InputsNPC controller)
    {
        RaycastHit hit;

        if (Physics.SphereCast(controller.Eyes.position, controller.GetStats.LookSpereCastRadius,
            controller.Eyes.forward, out hit, controller.GetStats.LookSpereCastRange)
            && hit.collider.CompareTag("Player"))
        {
            controller.ChaseTarget = hit.transform;
            return true;
        }
        else return false;
    }
    
}

