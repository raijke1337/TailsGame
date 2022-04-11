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

[RequireComponent(typeof(Collider))]
public abstract class BaseTrigger : MonoBehaviour,IAppliesTriggers
{

    protected Collider _coll;
    [Inject]
    protected TriggersProjectilesManager _manager; // only works for pre-spawned items todo

    public List<string> TriggerEffectIDs;

    public event BaseUnitWithIDEvent TriggerApplicationRequestEvent;

    public virtual bool Enable
    { 
        get => _coll.enabled;
        set => _coll.enabled = value;
    }

    protected void Awake()
    {
        _coll = GetComponent<Collider>();
        _coll.isTrigger = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var comp = other.GetComponent<BaseUnit>();
        if (comp == null) return;
        foreach (var id in TriggerEffectIDs)
        {
            TriggerApplicationRequestEvent?.Invoke(id, comp);
        }
    }

    protected virtual void OnTriggerExit(Collider other) { }

}

