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
public abstract class BaseController : IStatsComponentForHandler
{
    protected List<TriggeredEffect> _activeEffects = new List<TriggeredEffect>();

    [SerializeField] private bool _isReady = false;

    public event SimpleEventsHandler<bool,IStatsComponentForHandler> ComponentChangedStateToEvent;
    
    protected void StateChangeCallback(bool val, IStatsComponentForHandler comp)
    {
        ComponentChangedStateToEvent?.Invoke(val, comp);
        //Debug.Log($"{this} called state change with value {val}");
    }
    public virtual bool IsReady 
    { 
        get => _isReady; 
        protected set
        {
            _isReady = value;
            StateChangeCallback(value, this);
        }
    }

    public void Ping()
    {
        IsReady = _isReady;
    }
    // used by inputs to properly register some components


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

    protected virtual StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
    {
        Debug.Log($"{this} was requested to select a StatValueContainer by effect {effect}, and nothing happened");
        return null;
    }
    public virtual void UpdateInDelta(float deltaTime)
    {
        HandleEffects(deltaTime);
    }
    public virtual void SetupStatsComponent()
    {
        Debug.Log(this + " was setup");
    }

}

