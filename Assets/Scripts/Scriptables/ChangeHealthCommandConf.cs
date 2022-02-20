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


[CreateAssetMenu(fileName = "new ChangeHealthCommand",
    menuName = "Configurations/Commands/Change Health Config", order = 1)]
public class ChangeHealthCommandConf : ScriptableObject
{
    [SerializeField]
    private HealthChangeCommandData[] _commands;

    public ChangeHealthCommand GetEffect(string ID)
    {
        var data = _commands.First(t => t.Data.ID == ID);
        return new ChangeHealthCommand(data.InitialV, data.RepeatedV, data.Delay, data.Data);
    }

    [Serializable]
    public struct HealthChangeCommandData
    {
        public float InitialV;
        public float RepeatedV;
        public float Delay;
        public EffectData Data;
    }

}

