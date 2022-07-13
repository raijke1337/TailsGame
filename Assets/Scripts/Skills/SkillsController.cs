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
public class SkillsController : IStatsComponentForHandler, INeedsEmpties
{
    private List<string> IDs;
    private Dictionary <CombatActionType, SkillControllerData> _skills;

    public WeaponSwitchEventHandler SwitchAnimationLayersEvent;
    public ItemEmpties Empties { get; }
    public SkillsController(ItemEmpties ie) => Empties = ie;

    public bool IsReady { get; private set; } = false;

    public void LoadSkills (List<string> skills)
    {
        IDs = new List<string>();
        IDs.AddRange(skills);
        if (IDs.Count > 0) IsReady = true;
    }


    public bool RequestSkill (CombatActionType type, out float cost)
    {
        var result = _skills[type].RequestUse();
        cost = _skills[type].GetSkillData.SkillCost;
        if (result)
        {
            switch (type)
            {
                case CombatActionType.MeleeSpecialQ:
                    SwitchAnimationLayersEvent?.Invoke(EquipItemType.MeleeWeap);
                    break;
                case CombatActionType.RangedSpecialE:
                    SwitchAnimationLayersEvent?.Invoke(EquipItemType.RangedWeap);
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
    public SkillData GetSkillDataByType(CombatActionType type) => _skills[type].GetSkillData;

}

