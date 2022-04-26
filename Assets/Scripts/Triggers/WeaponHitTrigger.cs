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
using Zenject;

public class WeaponHitTrigger : BaseTrigger
{
    [SerializeField] public List<string> TriggerEffectIDs;
    protected override void OnTriggerEnter(Collider other)
    {
        var comp = other.GetComponent<BaseUnit>();
        if (comp == null) return;
        foreach (var id in TriggerEffectIDs)
        {
            TriggerCallback(id, comp);
        }
    }
}

