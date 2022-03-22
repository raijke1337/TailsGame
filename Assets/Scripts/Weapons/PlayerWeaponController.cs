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
    public WeaponType GetCurrentlyUsedWeaponType { get; private set; } = WeaponType.None;
    public WeaponSwitchEventHandler WeaponSwitchEvent;

    protected override void Start()
    {
        base.Start();
        WeaponSwitchEvent += SwapItem;
    }

    public override bool UseWeaponCheck(WeaponType type)
    {
        if (GetCurrentlyUsedWeaponType != type)
        {
            WeaponSwitchEvent?.Invoke(type);
            GetCurrentlyUsedWeaponType = type;
        }
        return base.UseWeaponCheck(type);
    }

//    visual part
    private void SwapItem(WeaponType type)
    {
        GameObject on = _currentWeapons[type].GetObject();
        GameObject off;
        if (!_currentWeapons.ContainsKey(GetCurrentlyUsedWeaponType)) off = null;
        else off = _currentWeapons[GetCurrentlyUsedWeaponType].GetObject();

        if (type == WeaponType.Ranged)
        {
            on.transform.position = _rangedWeaponEmpty.position;
            on.transform.rotation = _rangedWeaponEmpty.rotation;
            on.transform.parent = _rangedWeaponEmpty;
        }
        if (type == WeaponType.Melee)
        {
            on.transform.position = _meleeWeaponEmpty.position;
            on.transform.rotation = _meleeWeaponEmpty.rotation;
            on.transform.parent = _meleeWeaponEmpty;
        }
        if (off == null) return;
        off.transform.position = _sheathedWeaponEmpty.position;
        off.transform.rotation = _sheathedWeaponEmpty.rotation;
        off.transform.parent = _sheathedWeaponEmpty;
    }


}

