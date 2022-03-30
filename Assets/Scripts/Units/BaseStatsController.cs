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
    private SerializableDictionaryBase <StatType,StatValueContainer> _stats = new SerializableDictionaryBase<StatType, StatValueContainer>();
    public IReadOnlyDictionary<StatType, StatValueContainer> GetBaseStats => _stats;


    private string _displayName;
    public string GetDisplayName => _displayName;

    private List<TriggeredEffect> _effects;

    public void SetupStatsComponent()
    {
        foreach (var v in _stats.Values) { v.Setup(); }
        _effects = new List<TriggeredEffect>();
    }

    public BaseStatsController (string ID)
    {
        var cfg = Extensions.GetAssetsFromPath<BaseStatsConfig>(Constants.Configs.c_BaseStatConfigsPath).First
            (t=>t.ID == ID);
        // default settings
        if (cfg == null) // default settings
        {
            cfg = Extensions.GetAssetsFromPath<BaseStatsConfig>(Constants.Configs.c_BaseStatConfigsPath).First
                        (t => t.ID == "default");
        }
        
        _stats.Add(StatType.Health, new StatValueContainer(cfg.Stats[StatType.Health]));
        _stats.Add(StatType.Shield, new StatValueContainer(cfg.Stats[StatType.Shield]));
        _stats.Add(StatType.ShieldRegenMultiplier, new StatValueContainer(cfg.Stats[StatType.ShieldRegenMultiplier]));
        _stats.Add(StatType.HealthRegen, new StatValueContainer(cfg.Stats[StatType.HealthRegen]));
        _stats.Add(StatType.Heat, new StatValueContainer(cfg.Stats[StatType.Heat]));
        _stats.Add(StatType.HeatRegen, new StatValueContainer(cfg.Stats[StatType.HeatRegen]));
        _stats.Add(StatType.MoveSpeed, new StatValueContainer(cfg.Stats[StatType.MoveSpeed]));

        _displayName = cfg.displayName;
    }


    #region handler
    // regeneration and degradation of stats goes here
    // also calculations of applied effects
    public void UpdateInDelta(float deltaTime)
    {
        HandleEffects(deltaTime);
        _stats[StatType.Health].ChangeCurrent(_stats[StatType.HealthRegen].GetCurrent() * deltaTime);
        _stats[StatType.Heat].ChangeCurrent(_stats[StatType.HeatRegen].GetCurrent() * deltaTime);

        // separate logic for shield
        // todo?
        // always approaches starting value
        var reqSh = _stats[StatType.Shield].GetStart();
        var curSh = _stats[StatType.Shield].GetCurrent();
        float result = curSh - reqSh;

        // example : current sh 50, req (start) 50
        // 0 = 50 - 50
        // 0 = 0
        // dont change shield
        if (result == 0 ) return;


        // example : current sh 10, req (start) 50
        // -40 = 10 - 50
        // -40 < 0
        // value approaches starting 
        // current / required is how far the current value is so....
        // Coefficient: -40 / 50 = -0.8
        // need to increase

        float value = curSh / reqSh;
        if (result > 0)
        {
            _stats[StatType.Shield].ChangeCurrent(-value * Time.deltaTime * _stats[StatType.ShieldRegenMultiplier].GetCurrent());
        }
        // soo..
        if (result < 0)
        {
            _stats[StatType.Shield].ChangeCurrent(value * Time.deltaTime * _stats[StatType.ShieldRegenMultiplier].GetCurrent());
        }
        // todo fix - doesnt regen at 0


    }
    #endregion
    #region effects
    private void HandleEffects(float deltaTime)
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
        if (effect.StatID != StatType.Health)
        {
            _effects.Add(effect);
            return;
        }        

        float currentShield = _stats[StatType.Shield].GetCurrent();
        if (currentShield == 0)
        {
            _effects.Add(effect);
        }
        else
        {
            // absorb a const % if it odesnt exceed current shield, rest goes to hp

            //ex. original effect is -100 hp initial, -20 repeated over 10 seconds each 2 seconds
            // current shield is 70 (0-100 range)

            float shieldChange;
            float healthChange;

            // direct parts here

            shieldChange = Mathf.Clamp(effect.InitialValue * Constants.Combat.c_shieldAbsorbMult, -currentShield, _stats[StatType.Shield].GetMax());
            // (-100 * 0.4) Clamp -40 from -70 minimum (all shield used) to max shield (100) in case it's a bonus
            // result is  -40 - full 40% is absorbed

            healthChange = effect.InitialValue - shieldChange;
            // this is the direct part of dmg that bypasses shield
            // -100 - -40 = -60
            effect.InitialValue = healthChange;

            // -20 * 0.4 = -8
            float shieldRep = effect.RepeatedValue * Constants.Combat.c_shieldAbsorbMult;
            _effects.Add(new TriggeredEffect(effect.ID, StatType.Shield, shieldChange, shieldRep, effect.RepeatApplicationDelay, effect.TotalDuration, null, effect.TargetType));

            effect.RepeatedValue = effect.RepeatedValue * (1 - Constants.Combat.c_shieldAbsorbMult);
            _effects.Add(effect);
        }       
    }
    #endregion
}
