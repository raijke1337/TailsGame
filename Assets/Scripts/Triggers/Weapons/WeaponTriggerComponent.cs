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

public class WeaponTriggerComponent : BaseTrigger
{
    public BaseUnit Owner { get; set; }
    protected List<string> TriggerEffectIDs;
    public SimpleEventsHandler HitSuccessEvent;


    public void SetTriggerIDS(IEnumerable<string> ids)
    {
        TriggerEffectIDs = new List<string>();
        foreach (string id in ids) { TriggerEffectIDs.Add(id); }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        var comp = other.GetComponent<BaseUnit>();
        if (comp == null) return;
        HitSuccessEvent?.Invoke();
        foreach (var id in TriggerEffectIDs)
        {
            TriggerCallback(id, comp, Owner);
        }
    }
}

