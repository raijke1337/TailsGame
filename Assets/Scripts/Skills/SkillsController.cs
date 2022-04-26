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
    private Dictionary <CombatActionType, SkillControllerData> _skills;

    public WeaponSwitchEventHandler SwitchAnimationLayersEvent; 

    public SkillsController(List<string> skills)
    {
        IDs = new List<string>();
        IDs.AddRange(skills);
    }


    public bool RequestSkill (CombatActionType type, out float cost)
    {
        var result = _skills[type].RequestUse();
        cost = _skills[type].GetCost;
        if (result)
        {
            switch (type)
            {
                case CombatActionType.MeleeSpecialQ:
                    SwitchAnimationLayersEvent?.Invoke(WeaponType.Melee);
                    break;
                case CombatActionType.RangedSpecialE:
                    SwitchAnimationLayersEvent?.Invoke(WeaponType.Ranged);
                    break;
            }
        }
        return result;
    }

    public void SetupStatsComponent()
    {
        _skills = new Dictionary<CombatActionType, SkillControllerData>();
        foreach (string id in IDs)
        {
            var cfg = Extensions.GetAssetsFromPath<SkillControllerDataConfig>(Constants.Configs.c_SkillConfigsPath).First(t => t.ID == id);
            var type = cfg.SkillType;
            _skills[type] =  new SkillControllerData(cfg);
        }
    }

    public void UpdateInDelta(float deltaTime)
    {
        foreach (var sk in _skills.Values)
        {
            sk.Ticks(deltaTime);
        }
    }

    public string GetSkillIDByType(CombatActionType type) => _skills[type].ID;

}

