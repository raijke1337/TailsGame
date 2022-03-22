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
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class BaseStatsController : IStatsComponentForHandler, IStatsAddEffects
{
    [SerializeField]
    private SerializableDictionaryBase <StatType,StatValueContainer> _stats;
    public IReadOnlyDictionary<StatType, StatValueContainer> GetBaseStats => _stats;


    private List<TriggeredEffect> _effects;

    public void Setup()
    {
        foreach (var v in _stats.Values) { v.Setup(); }
        _effects = new List<TriggeredEffect>();
    }

    // regeneration and degradation of stats goes here
    // also calculations of applied effects
    public void UpdateInDelta(float deltaTime)
    {
        HandleEffects(deltaTime);
        _stats[StatType.Health].ChangeCurrent(_stats[StatType.HealthRegen].GetCurrent() * deltaTime);
        _stats[StatType.Shield].ChangeCurrent(_stats[StatType.ShieldRegen].GetCurrent() * deltaTime);
        _stats[StatType.Heat].ChangeCurrent(_stats[StatType.HeatRegen].GetCurrent() * deltaTime);
    }


    private void HandleEffects(float deltaTime)
    {
        if (_effects.Count == 0 ) return;
        foreach (var ef in _effects)
        {
            // actual handling
            StatValueContainer stat = _stats[ef.StatID];
            // start effect
            if (!ef.InitialDone)
            {
                stat.ChangeCurrent(ef.InitialValue);
                ef.InitialDone = true;
            }
            // remove expired or instantaneous
            if (ef.TotalDuration <= 0f)
            {
                _effects.Remove(ef);
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

    public void AddTriggeredEffect(TriggeredEffect effect)
    {
        _effects.Add(effect);
    }
    public void AddTriggeredEffects(IEnumerable<TriggeredEffect> effects)
    {
        foreach (var ef in effects)
        {
            AddTriggeredEffect(ef);
        }
    }
}

