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

    protected void Start()
    {
        _coll = GetComponent<Collider>();
        _coll.isTrigger = true;
        if (_manager == null) Debug.Log($"Missing manager reference on {name}");
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseUnit>()!=null)
        {
            var tgt = other.GetComponent<BaseUnit>();
            foreach (var id in TriggerEffectIDs)
            {
                _manager.Activation(id, tgt);
                Debug.Log($"Applying effect ID {id} to {other.name}");
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other) { }

}

