using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriggersProjectilesManager : MonoBehaviour
{
    // transform, destroy
    private List<IProjectile> _projectiles = new List<IProjectile>();

    //private List<BaseStatTriggerConfig> _configs = new List<BaseStatTriggerConfig>();
    //private List<ProjectileDataConfig> _projectileCfgs = new List<ProjectileDataConfig>();

    private Dictionary<string, BaseStatTriggerConfig> _triggersD = new Dictionary<string, BaseStatTriggerConfig>();
    private Dictionary<string, ProjectileDataConfig> _projectilesD = new Dictionary<string, ProjectileDataConfig>();


    private SkillsPlacerManager _skillsMan;

    private void Start()
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
        _skillsMan = GetComponent<SkillsPlacerManager>();
        _skillsMan.ProjectileSkillCreatedEvent += NewProjectile;
        _skillsMan.SkillAreaPlacedEvent += RegisterTrigger;

    }


    private void ApplyTriggerEffect(string ID, BaseUnit target, BaseUnit source)
    {
        var config = _triggersD[ID];
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

        finaltgt.ApplyEffect(effect);
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
        proj.ExpiryEventProjectile += ProjectileExpiryHandling;
        proj.OnSpawnProj();
    }

    private void ProjectileExpiryHandling(IProjectile proj)
    {
        _projectiles.Remove(proj);
        proj.OnExpiryProj();
    }
    private void Update()
    {
        foreach (var p in _projectiles.ToList())
        {
            if (p == null)
            {
                _projectiles.Remove(p);
                return;
            }
            p.OnUpdateProj();
        }
    }


    [ContextMenu(itemName: "Update configurations")]
    public void UpdateDatas()
    {
        var configs = Extensions.GetAssetsFromPath<BaseStatTriggerConfig>(Constants.Configs.c_TriggersConfigsPath,true);
        foreach (var cfg in configs)
        {
            _triggersD[cfg.ID] = cfg;
        }
        var projectileCfgs = Extensions.GetAssetsFromPath<ProjectileDataConfig>(Constants.Configs.c_ProjectileConfigsPath);
        foreach (var cfg in projectileCfgs)
        {
            _projectilesD[cfg.ID] = cfg;
        }
    }

}

