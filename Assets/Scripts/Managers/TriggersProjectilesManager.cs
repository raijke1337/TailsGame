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
using Zenject;

public class TriggersProjectilesManager : MonoBehaviour
{
    // transform, destroy
    private List<IProjectile> _projectiles = new List<IProjectile>();
    private PlayerUnit player;

    [SerializeField] private List<BaseStatTriggerConfig> _configs = new List<BaseStatTriggerConfig>();
    [SerializeField] private List<ProjectileDataConfig> _projectileCfgs = new List<ProjectileDataConfig>();

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

        player = _skillsMan.GetUnitsManager.GetPlayerUnit();
    }


    private void ApplyTriggerEffect(string ID, BaseUnit target)
    {
        var config = _configs.First(t => t.ID == ID);

        switch (config.SourceType)
        {
            case TriggerSourceType.Player:
                if (config.StatID == StatType.Heat || config .StatID == StatType.HeatRegen)
                {
                    target = player;
                }
                target.ApplyEffect(new TriggeredEffect(config));
                break;
            case TriggerSourceType.Enemy:
                if (target is NPCUnit) return;
                else target.ApplyEffect(new TriggeredEffect(config));
                break;
        }
    }

    private void RegisterTrigger(IAppliesTriggers item)
    {
        item.TriggerApplicationRequestEvent += ApplyTriggerEffect;
    }
    private void NewProjectile (IProjectile proj, string ID)
    {
        _projectiles.Add(proj);
        proj.SetProjectileData(_projectileCfgs.First(t => t.ID == ID));
        RegisterTrigger(proj);
        proj.ExpiryEventProjectile += ProjectileExpiryHandling;
        proj.OnSpawnProj();
    }

    private void ProjectileExpiryHandling(IProjectile proj)
    {
        _projectiles.Remove(proj);
        proj.TriggerApplicationRequestEvent -= (t1, t2) => ProjectileCollisionHandling(t1, t2, proj);
        proj.OnExpiryProj();
    }

    private void ProjectileCollisionHandling(string triggerId, BaseUnit target, IProjectile projectile)
    {
        ApplyTriggerEffect(triggerId, target);
    }

    private void Update()
    {
        foreach (var p in _projectiles.ToList())
        {
            if (p == null) return;
            p.OnUpdateProj();
        }
    }


    [ContextMenu(itemName: "Update configurations")]
    public void UpdateDatas()
    {
        _configs = Extensions.GetAssetsFromPath<BaseStatTriggerConfig>(Constants.Configs.c_TriggersConfigsPath,true);
        _projectileCfgs = Extensions.GetAssetsFromPath<ProjectileDataConfig>(Constants.Configs.c_ProjectileConfigsPath);
    }

}

