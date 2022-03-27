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
using Zenject;

public class GameManager : MonoInstaller
{    

    public override void Start()
    {
        base.Start();        
    }
    public override void InstallBindings()
    {
        Container.BindInstance(FindObjectOfType<PlayerUnit>()).AsSingle();
        Container.BindInstance(FindObjectOfType<StatsUpdatesHandler>()).AsSingle();
        Container.BindInstance(GetComponent<TriggersManager>()).AsSingle();
        Container.BindInstance(GetComponent<UnitsManager>()).AsSingle();
    }




}

