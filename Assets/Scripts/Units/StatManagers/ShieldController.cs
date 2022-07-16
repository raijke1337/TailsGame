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
public class ShieldController : BaseController, IStatsComponentForHandler, IGivesSkills, ITakesTriggers
{
    public Dictionary<ShieldStatType, StatValueContainer> GetShieldStats { get; private set; }

    public override bool IsReady { get; protected set; } = false;
    public IEquippable EquippedShieldItem { get; private set; }
    public ItemEmpties Empties { get; }
    public ShieldController(ItemEmpties ie) => Empties = ie;
    public void LoadItem(IEquippable item)
    {
        if (item.ItemContents.ItemType == EquipItemType.Shield)
        {
            EquippedShieldItem = item;
            IsReady = true;
        }
    }
    public IEnumerable<string> GetSkillStrings()
    {
        if (!IsReady) return null;
        else return new string[1] { EquippedShieldItem.ItemContents.SkillString };
    }



    public override void SetupStatsComponent()
    {
        var cfg = Extensions.GetConfigByID<ShieldSettings>(EquippedShieldItem.ItemContents.ID);

        if (cfg == null) return;

        var _keys = cfg.Stats.Keys.ToArray();
        var _values = cfg.Stats.Values.ToArray();
        GetShieldStats = new Dictionary<ShieldStatType, StatValueContainer>();

        for (int i = 0; i < _keys.Count(); i++)
        {
            GetShieldStats.Add(_keys[i], _values[i]);
        }
        foreach (var cont in GetShieldStats.Values)
        {
            cont.Setup();
        }
    }

    public override void UpdateInDelta(float deltaTime)
    {
        base.UpdateInDelta(deltaTime);
        GetShieldStats[ShieldStatType.Shield].ChangeCurrent(GetShieldStats[ShieldStatType.ShieldRegen].GetCurrent * deltaTime * GetShieldStats[ShieldStatType.ShieldRegenMultiplier].GetCurrent); 
    }

    public TriggeredEffect ProcessHealthChange(TriggeredEffect effect)
    {
        if (effect.InitialValue >= 0f)
        {
            return effect;
        }
        else
        {
            var adjDmg = effect.InitialValue * GetShieldStats[ShieldStatType.ShieldAbsorbMult].GetCurrent;
            effect.InitialValue -= adjDmg;
            var AdjRep = effect.RepeatedValue * GetShieldStats[ShieldStatType.ShieldAbsorbMult].GetCurrent;
            effect.RepeatedValue -= AdjRep;

            TriggeredEffect _shieldAbsord = new TriggeredEffect(effect.ID, effect.StatType, adjDmg,AdjRep,effect.RepeatApplicationDelay,effect.TotalDuration,effect.Icon);
            _activeEffects.Add(_shieldAbsord);

            return effect;
        }
    }

    protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
    {
        return GetShieldStats[ShieldStatType.Shield];
    }
}

