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
    public SimpleEventsHandler<WeaponType> WeaponSwitchEvent;

    protected override void Start()
    {
        base.Start();
        foreach (var weap in _currentWeapons)
        {
            weap.Value.GetObject().transform.localScale = new Vector3 (100f,100f,100f);
            // huge todo and also yikes
            // items are scaled down because ROOT of prefab is scale 0.01 
            // and empties are attached to it
        }
    }

    public override bool UseWeaponCheck(WeaponType type)
    {
        if (GetCurrentlyUsedWeaponType != type)
        {
            Equip(_currentWeapons[type].GetObject(),true);
            Equip(_currentWeapons.First(t => t.Key != type).Value.GetObject(),false);
            // todo
            WeaponSwitchEvent?.Invoke(type);
        }
        return base.UseWeaponCheck(type);
    }

    private void Equip(GameObject item,bool IsEquip)
    {
        if (IsEquip)
        {
            item.transform.parent = _weaponEmpty;
            item.transform.position = _weaponEmpty.position;
            item.transform.parent.rotation = _weaponEmpty.rotation;
        }
        if (!IsEquip)
        {
            item.transform.parent = _sheathedWeaponEmpty;
            item.transform.position = _sheathedWeaponEmpty.position;
            item.transform.parent.rotation = _sheathedWeaponEmpty.rotation;
        }
    }

}

