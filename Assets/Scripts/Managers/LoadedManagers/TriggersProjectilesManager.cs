using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriggersProjectilesManager : LoadedManagerBase
{
    // transform, destroy
    private List<IProjectile> _projectiles = new List<IProjectile>();
    private Dictionary<string, BaseStatTriggerConfig> _triggersD = new Dictionary<string, BaseStatTriggerConfig>();
    private Dictionary<string, ProjectileDataConfig> _projectilesD = new Dictionary<string, ProjectileDataConfig>();

   // private SkillsPlacerManager _skillsMan;

    
    private List<BaseStatTriggerConfig> _list;


    #region LoadedManager
    public override void Initiate()
    {
        UpdateDatas();
        var rangedWeapons = FindObjectsOfType<RangedWeapon>();
        var baseTriggers = FindObjectsOfType<BaseTrigger>();

        foreach (var w in rangedWeapons)
        {
            w.PlacedProjectileEvent += NewProjectile;
        }
        foreach (var triggerplacer in baseTriggers)
        {
            triggerplacer.TriggerApplicationRequestEvent += ApplyTriggerEffect;
        }
        var _skillsMan = GameManager.Instance.GetGameControllers.SkillsPlacerManager;
        _skillsMan.ProjectileSkillCreatedEvent += NewProjectile;
        _skillsMan.SkillAreaPlacedEvent += RegisterTrigger;
    }

    public override void RunUpdate(float delta)
    {
        foreach (var p in _projectiles.ToList())
        {
            p.OnUpdate(delta);
        }
    }

    public override void Stop()
    {

    }
    #endregion

    private void ApplyTriggerEffect(string ID, BaseUnit target, BaseUnit source)
    {
        BaseStatTriggerConfig config = null;

        try
        {
            config = _triggersD[ID];
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogWarning($"Failed to apply trigger ID {ID} to {target.GetFullName} : {e.Message}");
            return;
        }

        TriggeredEffect effect = new TriggeredEffect(config);
        BaseUnit finaltgt = null;

        if (source is NPCUnit)
        {
            switch (config.TargetType)
            {
                case TriggerTargetType.TargetsEnemies:
                    if (target is PlayerUnit player)
                    {
                        finaltgt = player;
                    }
                    break;
                case TriggerTargetType.TargetsUser:
                    if (target == source)
                    {
                        finaltgt = source;
                    }
                    break;
                case TriggerTargetType.TargetsAllies:
                    if (target != source && target.Side == source.Side)
                    {
                        finaltgt = target;
                    }
                    break;
            }
        }
        if (source is PlayerUnit)
        {
            switch (config.TargetType)
            {
                case TriggerTargetType.TargetsEnemies:
                    if (target is NPCUnit enemy)
                    {
                        finaltgt = enemy;
                    }
                    break;
                case TriggerTargetType.TargetsUser:
                    finaltgt = source;
                    break;
                case TriggerTargetType.TargetsAllies:
                    if (target != source && target.Side == source.Side)
                    {
                        finaltgt = target;
                    }
                    break;
            }
        }
        else if (!(source is PlayerUnit) && !(source is NPCUnit)) // traps , could keep null maybe? dont like it
        {
            switch (config.TargetType)
            {
                case TriggerTargetType.TargetsEnemies:
                    if (target is PlayerUnit player)
                    {
                        finaltgt = player;
                    }
                    break;
                case TriggerTargetType.TargetsUser:
                    if (target == source)
                    {

                    }
                    break;
                case TriggerTargetType.TargetsAllies:
                    if (target != source && target is NPCUnit) // maybe use for aoe support npc abilities
                    {
                        finaltgt = target;
                    }
                    break;
            }
        }
        if (finaltgt == null)
        {
            return;
        }

        finaltgt.AddTriggeredEffect(effect);
    }

    private void RegisterTrigger(IAppliesTriggers item)
    {
        item.TriggerApplicationRequestEvent += ApplyTriggerEffect;
    }
    private void NewProjectile (IProjectile proj)
    {
        _projectiles.Add(proj);
        proj.SetProjectileData(_projectilesD[proj.GetID]);
        RegisterTrigger(proj);
        proj.OnUse();
        proj.HasExpiredEvent += DeleteExpired;
    }

    private void DeleteExpired(IExpires item)
    {
        item.HasExpiredEvent -= DeleteExpired;
        Destroy(item.GetObject());
        _projectiles.Remove(item as IProjectile);
    }

    [ContextMenu(itemName: "Update configurations")]
    public void UpdateDatas()
    {
        _list = new List<BaseStatTriggerConfig>();
        var configs = DataManager.Instance.GetAssetsOfType<BaseStatTriggerConfig>(Constants.Configs.c_AllConfigsPath);
        foreach (var cfg in configs)
        {
            _triggersD[cfg.ID] = cfg;
            _list.Add(cfg);
        }
        var projectileCfgs = DataManager.Instance.GetAssetsOfType<ProjectileDataConfig>(Constants.Configs.c_AllConfigsPath);
        foreach (var cfg in projectileCfgs)
        {
            _projectilesD[cfg.ID] = cfg;
        }

        
    }

}

