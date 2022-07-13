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
    private SerializableDictionaryBase<BaseStatType, StatValueContainer> _stats;
    public IReadOnlyDictionary<BaseStatType, StatValueContainer> GetBaseStats => _stats;


    public string GetDisplayName { get; }
    public string GetUnitID { get; }

    public bool IsReady { get; protected set; } = false;

    private List<TriggeredEffect> _effects;

    public void SetupStatsComponent()
    {
        _effects = new List<TriggeredEffect>();
        foreach (var v in _stats.Values) { v.Setup(); }
    }


    public BaseStatsController (string ID)
    {
        _stats = new SerializableDictionaryBase<BaseStatType, StatValueContainer>();
        var cfg = Extensions.GetAssetsFromPath<BaseStatsConfig>(Constants.Configs.c_BaseStatConfigsPath).First
            (t=>t.ID == ID);
        // default settings
        if (cfg == null) // default settings
        {
            cfg = Extensions.GetAssetsFromPath<BaseStatsConfig>(Constants.Configs.c_BaseStatConfigsPath).First
                        (t => t.ID == "default");
        }

        var _keys = cfg.Stats.Keys.ToArray();
        var _values = cfg.Stats.Values.ToArray();

        for (int i = 0; i < _keys.Count(); i++)
        {
            _stats.Add(_keys[i], new StatValueContainer(_values[i]));
        }

        GetDisplayName = cfg.displayName;
        GetUnitID = cfg.ID;
        IsReady = true;
    }
    #region handler
    // regeneration and degradation of stats goes here
    // also calculations of applied effects
    public void UpdateInDelta(float deltaTime)
    {
        HandleEffects(deltaTime);

    }
    #endregion
    #region effects
    protected void HandleEffects(float deltaTime)
    {
        if (_effects.Count == 0) return;
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
    // here we need to recalculate the damages if the unit has a shield 
    public void AddTriggeredEffect(TriggeredEffect effect)
    {
        _effects.Add(effect);
    }
    #endregion
}
