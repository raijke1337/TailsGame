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

    // find ally and move to them, support if low hp; attack player if high hp
    protected override void Fsm_AgressiveActionRequestSM(CombatActionType type)
    {
        //SwitchRanges((type == CombatActionType.Ranged));

        base.Fsm_AgressiveActionRequestSM(type);
    }

    protected override void Fsm_CombatPreparationSM()
    {
        // find ally
        _stateMachine.FocusUnit = UnitRoom.GetUnitForAI(UnitType.Big);
    }

    private void SwitchRanges(bool isSupporting)
    {
        if (isSupporting)
        {
            _stateMachine.NMAgent.stoppingDistance = _skillCtrl.GetSkillDataByType(CombatActionType.RangedSpecialE).FinalArea; // heal when in range
        }
        else
        {
            _stateMachine.NMAgent.stoppingDistance = _enemyStats.AttackRange;
        }
    }

    protected override void Fsm_AggroRequestedSM()
    {
        base.Fsm_AggroRequestedSM();
        SwitchRanges(false);
    }

}

