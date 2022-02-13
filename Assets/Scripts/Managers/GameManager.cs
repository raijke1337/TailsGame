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

    private InGameUIManager _uiMan;
    private void Awake()
    {
        _uiMan = FindObjectOfType<InGameUIManager>();
    }

    public override void InstallBindings()
    {
        var Player = FindObjectOfType<PlayerUnit>();
        Container.BindInstance(Player).AsSingle();
    }



}

