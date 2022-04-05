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
public abstract class BaseSkill : MonoBehaviour
{
    public string ID;
    public BaseUnit Source;



    private Collider _coll;

    protected abstract void OnEnable();
    protected abstract void OnDisable();
    protected abstract void Update();
    protected abstract void OnTriggerEnter();

    private void Awake()
    {
        _coll = GetComponent<Collider>();
        _coll.isTrigger = true;
    }

}

