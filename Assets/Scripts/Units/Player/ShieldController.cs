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
public class ShieldController : IStatsComponentForHandler
{
    public ShieldController(string ID)
    {
        settingsID = ID;
    }
    public string settingsID { get; }

    private List<TriggeredEffect> _effects = new List<TriggeredEffect>();
    private Dictionary<ShieldStatType, StatValueContainer> _stats = new Dictionary<ShieldStatType, StatValueContainer>();
    public IReadOnlyDictionary<ShieldStatType, StatValueContainer> GetShieldStats() => _stats;

    public void SetupStatsComponent()
    {
        var cfg = Extensions.GetAssetsFromPath<ShieldSettings>(Constants.Configs.c_ShieldConfigsPath).First(T => T.ID == settingsID);
        var _keys = cfg.Stats.Keys.ToArray();
        var _values = cfg.Stats.Values.ToArray();

        for (int i = 0; i < _keys.Count(); i++)
        {
            _stats.Add(_keys[i], _values[i]);
        }
        foreach (var cont in _stats.Values)
        {
            cont.Setup();
        }
    }

    public void UpdateInDelta(float deltaTime)
    {
        HandleEffects(deltaTime);
        _stats[ShieldStatType.Shield].ChangeCurrent(_stats[ShieldStatType.ShieldRegen].GetCurrent() * deltaTime * _stats[ShieldStatType.ShieldRegenMultiplier].GetCurrent());
    }


    public TriggeredEffect ProcessHealthChange(TriggeredEffect effect)
    {
        if (effect.InitialValue >= 0f)
        {
            return effect;
        }
        else
        {
            var adjDmg = effect.InitialValue * _stats[ShieldStatType.ShieldAbsorbMult].GetCurrent();
            effect.InitialValue -= adjDmg;
            var AdjRep = effect.RepeatedValue * _stats[ShieldStatType.ShieldAbsorbMult].GetCurrent();
            effect.RepeatedValue -= AdjRep;

            TriggeredEffect _shieldAbsord = new TriggeredEffect(effect.ID, effect.StatID, adjDmg,AdjRep,effect.RepeatApplicationDelay,effect.TotalDuration,effect.Icon);
            _effects.Add(_shieldAbsord);

            return effect;
        }
    }


    protected void HandleEffects(float deltaTime)
    {
        if (_effects.Count == 0) return;
        foreach (var ef in _effects)
        {
            // actual handling
            StatValueContainer stat = _stats[ShieldStatType.Shield];
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

}

