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

public class ExplosiveProjectileSkill : BaseSkill
{
    protected override void OnDisable()
    {
    }

    protected override void OnEnable()
    {
    }

    protected override void OnTriggerEnter()
    {

    }

    protected override void Update()
    {
        transform.position += transform.forward * Time.deltaTime;
    }
}

