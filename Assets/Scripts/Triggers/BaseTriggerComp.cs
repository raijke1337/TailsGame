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

[RequireComponent(typeof(Collider))]
public abstract class BaseTriggerComp : MonoBehaviour
{
    private Collider _collider;

    public bool Enabled
    {
        get => _collider.enabled;
        set => _collider.enabled = value;
    }
    protected virtual void Start()
    {
        _collider = this.GetComponent<Collider>();
        _collider.isTrigger = true;
    }
    protected abstract void OnTriggerEnter(Collider other);
    protected virtual void OnTriggerExit(Collider other)
    {

    }
}

