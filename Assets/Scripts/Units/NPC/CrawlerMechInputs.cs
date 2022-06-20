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

public class CrawlerMechInputs : InputsNPC
{
    // find ally and run to them, then fight
    protected override void Fsm_CombatPreparationSM()
    {
        fsm.FocusUnit = UnitRoom.GetUnitForAI(UnitType.Big); 
        fsm.SelectedUnit = UnitRoom.GetUnitForAI(UnitType.Player);   
    }

    protected override void HandleAttackRequest(CombatActionType type)
    {
        base.HandleAttackRequest(type);
    }

    protected override void Fsm_AggroRequestedSM()
    {
        fsm.FocusUnit = fsm.StateMachineUnit; // for self-destruct skill
        base.Fsm_AggroRequestedSM();

    }
}

