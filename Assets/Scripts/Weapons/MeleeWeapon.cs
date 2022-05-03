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

[RequireComponent(typeof(WeaponTrigger))]
public class MeleeWeapon : BaseWeapon
{
    private WeaponTrigger _trigger;
    private void Start()
    {
        _trigger = GetComponent<WeaponTrigger>();
        _trigger.SetTriggerIDS(_effectsIDs);
        _trigger.Enable = false;
    }


    public void ToggleColliders(bool enable)
    {
        _trigger.Enable = enable;
    }

    public override bool UseWeapon()
    {
        return true;
    }
    // swings deplete charges
}

