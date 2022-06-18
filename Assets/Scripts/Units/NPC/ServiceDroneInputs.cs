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
        RotateToSelectedUnit();
        if (AllyHP!= null && AllyHP.GetCurrent/AllyHP.GetMax != 0) // TODO
        {
            _skillCtrl.RequestSkill(CombatActionType.Ranged,out float c);
        }
        _weaponCtrl.UseWeaponCheck(WeaponType.Ranged);
    }

    protected override void Fsm_CombatPreparationSM()
    {
        fsm.SelectedUnit = UnitRoom.GetUnitForAI(EnemyType.Big);

        if (fsm.SelectedUnit.Side == fsm.Unit.Side) AllyHP = fsm.SelectedUnit.GetStats()[BaseStatType.Health];
    }
}

