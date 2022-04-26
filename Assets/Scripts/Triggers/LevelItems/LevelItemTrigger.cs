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

public class LevelItemTrigger : BaseTrigger
{
    public string TriggerID;

    [SerializeField,Space] protected ParticleSystem _activateEffect;

    protected override void OnTriggerEnter(Collider other)
    {
        if (_activateEffect == null) return;
        _activateEffect.enableEmission = true;
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (_activateEffect == null) return;
        _activateEffect.enableEmission = false;
    }
}

