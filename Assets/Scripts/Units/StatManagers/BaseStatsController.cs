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
public class BaseStatsController : BaseController, IStatsComponentForHandler, ITakesTriggers
{
    public SerializableDictionaryBase<BaseStatType, StatValueContainer> GetBaseStats { get;private set; }
    public event SimpleEventsHandler UnitDiedEvent;
    public string GetDisplayName { get; }


    #region ihandler
    public override void SetupStatsComponent()
    {
        foreach (var v in GetBaseStats.Values) { v.Setup(); }
    }
    public override void UpdateInDelta(float deltaTime)
    {
        base.UpdateInDelta(deltaTime);
        if (GetBaseStats[BaseStatType.Health].GetCurrent <= 0f) UnitDiedEvent?.Invoke();
    }
    #endregion


    public BaseStatsController (string ID)
    {
        GetBaseStats = new SerializableDictionaryBase<BaseStatType, StatValueContainer>();
        var cfg = Extensions.GetConfigByID<BaseStatsConfig>(ID);

        if (cfg == null) return;

        var _keys = cfg.Stats.Keys.ToArray();
        var _values = cfg.Stats.Values.ToArray();

        for (int i = 0; i < _keys.Count(); i++)
        {
            GetBaseStats.Add(_keys[i], new StatValueContainer(_values[i]));
        }

        GetDisplayName = cfg.displayName;
        IsReady = true;
    }


    protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
    {
        StatValueContainer result = null;
        switch (effect.StatType)
        {
            case TriggerChangedValue.Health:
                result = GetBaseStats[BaseStatType.Health];
                break;
            case TriggerChangedValue.Shield:
                break;
            case TriggerChangedValue.Combo:
                break;
            case TriggerChangedValue.MoveSpeed:
                result = GetBaseStats[BaseStatType.MoveSpeed];
                break;
            case TriggerChangedValue.TurnSpeed:
                result = GetBaseStats[BaseStatType.TurnSpeed];
                break;
            case TriggerChangedValue.Stagger:
                break;
        }
        return result;
    }

}
