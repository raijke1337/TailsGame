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

public class InGameManager : MonoInstaller
{

    [SerializeField] private CursorManager _cursors;

    private UnitsManager UnitsManager { get; set; }
    public PlayerUnit GetPlayer => UnitsManager.GetPlayerUnit;

    private void Awake()
    {
        var config = Extensions.GetConfigByID<CursorsDictionary>("default");
        _cursors = new CursorManager(config);
        
        UnitsManager = GetComponent<UnitsManager>();
    }

    public override void InstallBindings()
    {
        Container.BindInstance(FindObjectOfType<PlayerUnit>()).AsSingle();
        Container.BindInstance(FindObjectOfType<StatsUpdatesHandler>()).AsSingle();
        Container.BindInstance(GetComponent<TriggersProjectilesManager>()).AsSingle();
        Container.BindInstance(GetComponent<UnitsManager>()).AsSingle();
    }


}

