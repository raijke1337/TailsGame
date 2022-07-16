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
public class ComboController : BaseController, IStatsComponentForHandler, ITakesTriggers
{

    public StatValueContainer ComboContainer { get; }

    public override bool IsReady { get; protected set; } = false;

    protected float Degen;
    protected float Timeout;
    private float _currentTimeout = 0f;

    public ComboController(string ID)
    {
        var cfg = Extensions.GetConfigByID<ComboStatsConfig>(ID);
        if (cfg == null) return;
        IsReady = true;

        ComboContainer = new StatValueContainer(cfg.ComboContainer);
        Degen = cfg.DegenCoeff;
        Timeout = cfg.HeatTimeout;
    }
    // todo replace with triggers
    public void AddCombo(float value)
    {
        ComboContainer.ChangeCurrent(value);
        _currentTimeout = 0f;
    }

    public bool UseCombo(float value)
    {
        bool result = ComboContainer.GetCurrent >= -value;    // because these are negative in configs
        if (result) ComboContainer.ChangeCurrent(value);
        return result;
    }

    public override void SetupStatsComponent()
    {
        ComboContainer.Setup();
    }
    public override void UpdateInDelta(float deltaTime)
    {

        base.UpdateInDelta(deltaTime);
        if (_currentTimeout <= Timeout)
        {
            _currentTimeout += deltaTime;
            return;
        }
        ComboContainer.ChangeCurrent(-Degen * deltaTime);
    }

    protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
    {
        return ComboContainer;  
    }
}

