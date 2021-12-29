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
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class BaseTrigger : BaseStats, IInteractiveItem
{

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }      


    public virtual bool Interact()
    {
        _animator.SetTrigger("Activation");
        return true;
    }

}

