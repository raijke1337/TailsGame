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

public class PlayerWeaponController : BaseWeaponController
{
    public WeaponSwitchEventHandler WeaponSwitchEvent; // also used for layers switch in playerunit

    public override void SetupStatsComponent()
    {
        base.SetupStatsComponent();
    }
    public override bool UseWeaponCheck(WeaponType type)
    {
        if (CurrentWeaponType != type)
        {
            SwapItem(type);
        }
        return base.UseWeaponCheck(type);
    }

    private void SwapItem(WeaponType type)
    {
        foreach (var w in _currentWeapons.Keys)
        {
            if (w == type)
            {
                EquipItem(type);
            }
            else UnequipItem(type);
        }
    }
}

