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
        fsm.SelectedUnit = UnitRoom.GetUnitForAI(EnemyType.Big); 
        if (fsm.SelectedUnit == null) UnitRoom.GetUnitForAI(EnemyType.Small, true);  // no ally found, set player        
    }

    protected override void HandleAttackRequest(CombatActionType type)
    {// todo
        _weaponCtrl.UseWeaponCheck(WeaponType.Melee);
    }
}

