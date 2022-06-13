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

    protected override void HandleAttackRequest()
    {
        if (AllyHP!= null && AllyHP.GetCurrent/AllyHP.GetMax != 0) // TODO
        {
            _skillCtrl.RequestSkill(CombatActionType.Ranged,out float c);
        }
        _weaponCtrl.UseWeaponCheck(WeaponType.Ranged);
    }

    protected override void Fsm_CombatPreparationSM()
    {
        fsm.OnRequestAlly();
        if (fsm.FoundAlly != null) AllyHP = fsm.FoundAlly.GetStats()[BaseStatType.Health];
    }
}

