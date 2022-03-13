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

public class GameManager : MonoInstaller, GameManager.IManager
{

    [SerializeField]
    private ScriptableObject[] _commConfigs;
    private TriggersManager _triggers;

    public override void Start()
    {
        base.Start();        
    }
    public override void InstallBindings()
    {
        var Player = FindObjectOfType<PlayerUnit>();
        Container.BindInstance(Player).AsSingle();
        Container.BindInstances(_commConfigs);

        //Container.BindInstance(_triggers).AsSingle();
    }

    [ContextMenu("Load all configs")]
    public void LoadConfigurations()
    {
        var paths = Directory.GetFiles(Application.dataPath + "/Scripts/Configurations/CommandTriggers", "*.asset");
        var configs = new LinkedList<ScriptableObject>();

        foreach (var path in paths)
        {
            var newpath = "Assets" + path.Replace(Application.dataPath, "").Replace("\\", "/");
            var config = AssetDatabase.LoadAssetAtPath(newpath, typeof(UnityEngine.Object)) as ScriptableObject;
            configs.AddLast(config);
        }
        _commConfigs = configs.ToArray();
        var managers = FindObjectsOfType<MonoBehaviour>().Where(t => t is IManager && t != this);
        foreach (IManager m in managers)
        {
            m.LoadConfigurations();
        }
    }


    internal interface IManager
    {
        void LoadConfigurations();
    }

}

