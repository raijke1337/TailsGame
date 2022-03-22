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

[RequireComponent(typeof(WeaponHitTrigger))]
public class MeleeWeapon : BaseWeapon
{
    private WeaponHitTrigger _trigger;
    private void Start()
    {
        _trigger = GetComponent<WeaponHitTrigger>();
        _trigger.TriggerEffectIDs = _effectsIDs;
    }

    public override bool UseWeapon()
    {
        return true;
    }
    // swings deplete charges
}

