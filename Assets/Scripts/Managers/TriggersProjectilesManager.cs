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


    [SerializeField] private List<BaseStatTriggerConfig> _configs = new List<BaseStatTriggerConfig>();
    [SerializeField] private List<ProjectileDataConfig> _projectileCfgs = new List<ProjectileDataConfig>();

    [Inject]
    private PlayerUnit _player;

    private SkillsPlacerManager _skillsMan;

    private void Start()
    {
        UpdateDatas();
        var rangedWeapons = FindObjectsOfType<RangedWeapon>();
        foreach (var w in rangedWeapons)
        {
            w.PlacedProjectileEvent += NewProjectile;
        }
        _skillsMan = GetComponent<SkillsPlacerManager>();
        _skillsMan.ProjectileSkillCreatedEvent += NewProjectile;
    }

    // todo??
    // here we send the original triggered effect to unit
    public void ApplyTriggerEffect(string ID, BaseUnit target)
    {
        var config = _configs.First(t => t.ID == ID);

        switch (config.TargetType)
        {
            case TriggeredEffectTargetType.Target:
                target.ApplyEffect(new TriggeredEffect(config));
                break;

            case TriggeredEffectTargetType.Self:
                _player.ApplyEffect(new TriggeredEffect(config));
                break;
        };
    }

    private void NewProjectile (IProjectile proj, string ID)
    {
        _projectiles.Add(proj);
        proj.SetProjectileData(_projectileCfgs.First(t => t.ID == ID));
        proj.TriggerHitEvent += (t1, t2) => ProjectileCollisionHandling(t1, t2, proj);
        proj.ExpiryEvent += ProjectileExpiryHandling;
        proj.OnSpawn();
    }

    private void ProjectileExpiryHandling(IProjectile arg)
    {
        _projectiles.Remove(arg);
        arg.TriggerHitEvent -= (t1, t2) => ProjectileCollisionHandling(t1, t2, arg);
        arg.OnExpiry();
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
            p.OnUpdate();
        }
    }


    [ContextMenu(itemName: "Update configurations")]
    public void UpdateDatas()
    {
        _configs = Extensions.GetAssetsFromPath<BaseStatTriggerConfig>(Constants.Configs.c_TriggersConfigsPath);
        _projectileCfgs = Extensions.GetAssetsFromPath<ProjectileDataConfig>(Constants.Configs.c_ProjectileConfigsPath);
    }

}

