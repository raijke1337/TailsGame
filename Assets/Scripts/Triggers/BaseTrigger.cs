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
public abstract class BaseTrigger : MonoBehaviour
{
    
    protected Collider _coll;
    [Inject]
    protected TriggersManager _manager;

    public string TriggerEffectID;

    public bool Enable
    {
        get => _coll.enabled;
        set => _coll.enabled = value;
    }

    protected void Start()
    {
        _coll = GetComponent<Collider>();
        _coll.isTrigger = true;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseUnit>()!=null)
        {
            var tgt = other.GetComponent<BaseUnit>();
            _manager.Activation(TriggerEffectID,tgt);

            Debug.Log($"Applying effect ID {TriggerEffectID} to {other.name}");
        }
    }

    protected virtual void OnTriggerExit(Collider other) { }

}

