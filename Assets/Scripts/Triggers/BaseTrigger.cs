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
    public event TriggerEventApplication TriggerApplicationRequestEvent;
    protected void TriggerCallback(string ID, BaseUnit unit,BaseUnit source) => TriggerApplicationRequestEvent?.Invoke(ID,unit,source);

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

    protected abstract void OnTriggerEnter(Collider other);

    protected virtual void OnTriggerExit(Collider other) { }
}

