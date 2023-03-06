using ModestTree;
using System;
using System.Collections.Generic;

[Serializable]
public class SkillsController : BaseController, INeedsEmpties
{
    // this is used in game for skill requests
    private Dictionary<CombatActionType, SkillControllerData> _skills = new Dictionary<CombatActionType, SkillControllerData>();

    public WeaponSwitchEventHandler SwitchAnimationLayersEvent;
    public ItemEmpties Empties { get; }
    public SkillsController(ItemEmpties ie) => Empties = ie;


    public void UpdateSkills(string skillID, bool isAdd)
    {
        var cfg = DataManager.Instance.GetConfigByID<SkillControllerDataConfig>(skillID);
        if (cfg == null) { return; }
        if (_skills == null) _skills = new Dictionary<CombatActionType, SkillControllerData>();
        else
        {
            var type = cfg.SkillType;
            _skills[type] = new SkillControllerData(cfg);
        }
        IsReady = _skills.HasAtLeast(1);
    }


    public bool RequestSkill(CombatActionType type, out float cost)
    {
        cost = 0f;
        if (!_skills.ContainsKey(type)) return false;

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

    public override void SetupStatsComponent()
    { }

    public override void UpdateInDelta(float deltaTime)
    {
        foreach (var sk in _skills.Values)
        {
            sk.Ticks(deltaTime);
        }
    }

    public string GetSkillIDByType(CombatActionType type) => _skills[type].ID;
    public SkillData GetSkillDataByType(CombatActionType type) => _skills[type].GetSkillData;

}



