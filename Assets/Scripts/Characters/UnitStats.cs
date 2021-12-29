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
using UnityEngine.EventSystems;

public class UnitStats : BaseStats
{

    public int maxHP = 100;
    public int currHP;
    public float MS = 3f;
    public Allegiance Side;

    public event SimpleEventsHandler ZeroHealthEvent;
    private bool Dead;

    private void Start()
    {
        currHP = maxHP;
    }
    private void Update()
    {
        if (!Dead)
        {
            if (currHP <= 0)
            {
                Dead = true;
                ZeroHealthEvent?.Invoke();
            }
        }
    }


}

