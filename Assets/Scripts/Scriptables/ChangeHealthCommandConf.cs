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
public class ChangeHealthCommandConf : EffectConfiguration<ChangeHealthCommand>
{
    [SerializeField]
    protected List<float> _firstDm;
    [SerializeField]
    protected List<float> _repeatedDm;
    [SerializeField]
    protected List<float> _delays;


    public override ChangeHealthCommand CreateEffect(string ID)
    {
        int i = 0;
        for (; i < _mainDatas.Count; i++)
        {
            if (_mainDatas[i].ID == ID) break;
        }
        return new ChangeHealthCommand(
            initalV: _firstDm[i], repeatedV: _repeatedDm[i], delay: _delays[i], _mainDatas[i]);
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

