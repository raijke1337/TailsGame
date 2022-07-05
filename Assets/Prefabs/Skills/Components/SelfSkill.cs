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

public class SelfSkill : BaseSkill
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (Source == null) return; // test

        if(other.CompareTag(Source.tag)) base.OnTriggerEnter(other);
    }
}

