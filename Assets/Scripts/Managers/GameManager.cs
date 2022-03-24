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
    private PlayerUnit _player;
    private StatsUpdatesHandler _statsH;
    private TriggersManager _triggers;

    

    public override void Start()
    {
        base.Start();        
    }
    public override void InstallBindings()
    {
        _player = FindObjectOfType<PlayerUnit>();
        _statsH = FindObjectOfType<StatsUpdatesHandler>();
        _triggers = GetComponent<TriggersManager>();
        Container.BindInstance(_player).AsSingle();
        Container.BindInstance(_statsH).AsSingle();
        Container.BindInstance(_triggers).AsSingle();
    }




}

