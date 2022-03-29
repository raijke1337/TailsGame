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
[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    public override void Act(NPCUnitControllerAI controller)
    {
        Attack(controller);
    }


    private void Attack(NPCUnitControllerAI controller)
    {
        RaycastHit hit;

        if (Physics.SphereCast(controller.Eyes.position, controller.GetStats.LookSpereCastRadius,
            controller.Eyes.forward, out hit, controller.GetStats.AttackRange) &&
            hit.collider.CompareTag("Player"))
        {
            if (controller.IsAttackReady())
            {
                controller.AttackRequest();
            }
        }
    }
}

