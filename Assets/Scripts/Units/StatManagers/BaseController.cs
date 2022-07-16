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

public abstract class BaseController : IStatsComponentForHandler
{
    protected List<TriggeredEffect> _activeEffects = new List<TriggeredEffect>();

    public abstract bool IsReady { get; protected set; }

    public virtual void HandleEffects(float deltaTime)
    {
        if (_activeEffects.Count == 0) return;
        foreach (var ef in _activeEffects)
        {
            // actual handling
            StatValueContainer stat = SelectStatValueContainer(ef);
            // start effect
            if (!ef.InitialDone)
            {
                stat.ChangeCurrent(ef.InitialValue);
                ef.InitialDone = true;
            }
            // remove expired or instantaneous
            if (ef.TotalDuration <= 0f)
            {
                _activeEffects.Remove(ef);
                return;
            }
            // handle timers    
            ef.CurrentRepeatTimer -= deltaTime;
            ef.TotalDuration -= deltaTime;
            if (ef.CurrentRepeatTimer <= 0f)
            {
                stat.ChangeCurrent(ef.RepeatedValue);
                ef.CurrentRepeatTimer = ef.RepeatApplicationDelay;
            }
        }
    }

    public virtual void AddTriggeredEffect(TriggeredEffect effect)
    {
        _activeEffects.Add(effect);
    }

    protected abstract StatValueContainer SelectStatValueContainer(TriggeredEffect effect);
    public virtual void UpdateInDelta(float deltaTime)
    {
        HandleEffects(deltaTime);
    }
    public abstract void SetupStatsComponent();
}

