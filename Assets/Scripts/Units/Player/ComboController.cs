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
[Serializable]
public class ComboController : IStatsComponentForHandler
{

    public StatValueContainer ComboContainer { get; }

    protected float Degen;
    protected float Timeout;
    private float _currentTimeout = 0f;

    public ComboController(string ID)
    {
        var cfg = Extensions.GetAssetsFromPath<ComboStatsConfig>(Constants.Configs.c_ComboConfigsPath).First(t => t.ID == ID);
        ComboContainer = new StatValueContainer(cfg.ComboContainer);
        Degen = cfg.DegenCoeff;
        Timeout = cfg.HeatTimeout;
    }

    public void AddCombo(float value)
    {
        ComboContainer.ChangeCurrent(value);
        _currentTimeout = 0f;
    }

    public bool UseCombo(float value)
    {
        bool result = ComboContainer.GetCurrent() >= value;    
        if (result) ComboContainer.ChangeCurrent(value);
        return result;
    }

    public void SetupStatsComponent()
    {
        ComboContainer.Setup();
    }
    public void UpdateInDelta(float deltaTime)
    {
        if (_currentTimeout <= Timeout)
        {
            _currentTimeout += deltaTime;
            return;
        }
        ComboContainer.ChangeCurrent(-Degen * deltaTime);
    }
}

