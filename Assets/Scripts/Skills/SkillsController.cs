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
public class SkillsController : IStatsComponentForHandler
{
    [SerializeField] private List<string> IDs;
    private Dictionary<CombatActionType, BaseSkill> _skills;

    public SkillsController(List<string> skills)
    {
        IDs = new List<string>();
        IDs.AddRange(skills);
    }

    // crossbow_explosive
    // hammer_spin
    // shield_heal
    // use mono class for ingame activities
    // use SO config to set base values

    public bool RequestSkill (CombatActionType type)
    {
        return _skills[type].RequestUse();
    }

    public void SetupStatsComponent()
    {
        _skills = new Dictionary<CombatActionType, BaseSkill>();
        foreach (string id in IDs)
        {
            var cfg = Extensions.GetAssetsFromPath<BaseSkillSettings>(Constants.Configs.c_SkillConfigsPath).First(t => t.ID == id);
            var type = cfg.SkillType;
            _skills.Add(type, new BaseSkill(cfg));
        }
    }

    public void UpdateInDelta(float deltaTime)
    {
        foreach (var sk in _skills.Values)
        {
            sk.Ticks(deltaTime);
        }
    }



}

