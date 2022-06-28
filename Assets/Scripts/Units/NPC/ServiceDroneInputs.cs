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

public class ServiceDroneInputs : InputsNPC
{
    protected override void Fsm_ChangeRangeActionRequestSM(CombatActionType arg)
    {
        switch (arg)
        {
            case CombatActionType.Melee:
                break;
            case CombatActionType.Ranged:
                _stateMachine.NMAgent.stoppingDistance = _enemyStats.AttackRange;
                break;
            case CombatActionType.Dodge:
                break;
            case CombatActionType.MeleeSpecialQ:
                break;
            case CombatActionType.RangedSpecialE:
                _stateMachine.NMAgent.stoppingDistance = _skillCtrl.GetSkillDataByType(CombatActionType.RangedSpecialE).FinalArea / 2;
                break;
            case CombatActionType.ShieldSpecialR:
                break;
        }
    }
}