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
    protected BaseUnit tgt;

    //some logic here?
    [SerializeField] private ParticleSystem _activateEffect;
    protected override void OnTriggerEnter(Collider other)
    {
        tgt = other.GetComponent<PlayerUnit>();
        if (tgt != null)
        {
            foreach (var id in TriggerEffectIDs)
            {
                _manager.ApplyTriggerEffect(id, tgt);
            }
        }
    }
}

