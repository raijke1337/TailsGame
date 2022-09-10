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

[RequireComponent(typeof(WeaponTriggerComponent))]
public class MeleeWeapon : BaseWeapon
{
    private WeaponTriggerComponent _trigger;

    protected virtual void Start()
    {
        _trigger = GetComponent<WeaponTriggerComponent>();
        _trigger.SetTriggerIDS(_effectsIDs);
        _trigger.Enable = false;
        _trigger.Owner = Owner;
    }


    public void ToggleColliders(bool enable)
    {
        _trigger.Enable = enable;
    }



}

