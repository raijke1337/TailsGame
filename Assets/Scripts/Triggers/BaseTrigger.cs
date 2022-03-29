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
    [Inject,SerializeField]
    protected TriggersManager _manager;

    public List<string> TriggerEffectIDs;


    public bool Enable
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
        if (other.GetComponent<BaseUnit>()!=null)
        {
            var tgt = other.GetComponent<BaseUnit>();
            foreach (var id in TriggerEffectIDs)
            {
                _manager.Activation(id, tgt);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other) { }

    public string GetName() => name;
}

