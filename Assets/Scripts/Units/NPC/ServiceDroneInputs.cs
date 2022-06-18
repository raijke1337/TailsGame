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
    protected StatValueContainer AllyHP;

    // find ally and move to them, support if low hp; attack player if high hp
    protected override void HandleAttackRequest(CombatActionType type)
    {
        //todo
        base.HandleAttackRequest(type);
    }

    protected override void Fsm_CombatPreparationSM()
    {
        fsm.SelectedUnit = UnitRoom.GetUnitForAI(UnitType.Big);

        if (fsm.SelectedUnit == null) fsm.SelectedUnit = UnitRoom.GetUnitForAI(UnitType.Player);

        if (fsm.SelectedUnit.Side == fsm.StateMachineUnit.Side) AllyHP = fsm.SelectedUnit.GetStats()[BaseStatType.Health];

    }
}

