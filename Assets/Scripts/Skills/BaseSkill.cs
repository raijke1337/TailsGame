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
    public string SkillID;
    public BaseUnit Source;

    public SkillData SkillData;

    public GameObject EffectPrefab;

    private Collider _coll;

    protected abstract void OnTriggerEnter(Collider other);


    private void Awake()
    {
        if (EffectPrefab == null || EffectPrefab.GetComponent<SkillAreaComp>() == null) Debug.LogError($"Set prefab for {this}");
        _coll = GetComponent<Collider>();
        _coll.isTrigger = true;
    }


}

