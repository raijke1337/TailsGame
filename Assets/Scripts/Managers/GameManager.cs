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

    [SerializeField]
    private ScriptableObject[] _configs;
    private TriggersManager _triggers;


    public override void InstallBindings()
    {
        var Player = FindObjectOfType<PlayerUnit>();
        Container.BindInstance(Player).AsSingle();
        Container.BindInstances(_configs);

        //Container.BindInstance(_triggers).AsSingle();
    }

    [ContextMenu("Load all configs")]
    private void GetAllConfigurations()
    {
        var paths = Directory.GetFiles(Application.dataPath + "/Scripts/Configurations", "*.asset");
        var configs = new LinkedList<ScriptableObject>();

        foreach (var path in paths)
        {
            var newpath = "Assets" + path.Replace(Application.dataPath, "").Replace("\\", "/");
            var config = AssetDatabase.LoadAssetAtPath(newpath, typeof(UnityEngine.Object)) as ScriptableObject;
            configs.AddLast(config);
        }
        _configs = configs.ToArray();
    }


}

