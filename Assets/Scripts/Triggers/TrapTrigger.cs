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

public class TrapTrigger : BaseTrigger
{
    //some logic here?
    [SerializeField] private ParticleSystem _activateEffect;
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        // _activateEffect.enableEmission = true;
        // todo

    }
}

