using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriggersProjectilesManager : MonoBehaviour
{
    // transform, destroy
    private List<IProjectile> _projectiles = new List<IProjectile>();

    private List<BaseStatTriggerConfig> _configs = new List<BaseStatTriggerConfig>();
    private List<ProjectileDataConfig> _projectileCfgs = new List<ProjectileDataConfig>();


#if UNITY_EDITOR
    [SerializeField] private Text _triggersDebug;
#endif



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

#if UNITY_EDITOR
        _triggersDebug.text = $"{this} started";
#endif

    }


    private void ApplyTriggerEffect(string ID, BaseUnit target, BaseUnit source)
    {
        var config = _configs.First(t => t.ID == ID);
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
#if UNITY_EDITOR
                        _triggersDebug.text = $"{effect} applied by ID {ID} source {source} targets user";
#endif
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
#if UNITY_EDITOR
            _triggersDebug.text += $"\n Trigger {ID} not applied";
#endif
            return;
        }



#if UNITY_EDITOR
        _triggersDebug.text += $"\n Applying effect ID {ID}, source: {source}, target {finaltgt}";
#endif

        finaltgt.ApplyEffect(effect);
    }

    // todo check the result of gettype

    //private void ApplyTriggerEffect(string ID, BaseUnit target, BaseUnit source)
    //{
    //    var config = _configs.First(t => t.ID == ID);

    //    switch (config.TargetType)
    //    {
    //        case TriggerTargetType.TargetsEnemies:
    //            if (target.GetType() != source.GetType())
    //            {
    //                
    //            }
    //            break;
    //        case TriggerTargetType.TargetsUser:
    //            break;
    //        case TriggerTargetType.TargetsAllies:
    //            break;
    //    }
    //}



    private void RegisterTrigger(IAppliesTriggers item)
    {
        item.TriggerApplicationRequestEvent += ApplyTriggerEffect;
    }
    private void NewProjectile (IProjectile proj)
    {
        _projectiles.Add(proj);
        proj.SetProjectileData(_projectileCfgs.First(t => t.ID == proj.GetID));
        RegisterTrigger(proj);
        proj.ExpiryEventProjectile += ProjectileExpiryHandling;
        proj.OnSpawnProj();
    }

    private void ProjectileExpiryHandling(IProjectile proj)
    {
        _projectiles.Remove(proj);
        //proj.TriggerApplicationRequestEvent -= (id, tgt) => ProjectileCollisionHandling(id, tgt, proj);
        proj.OnExpiryProj();
    }

    //private void ProjectileCollisionHandling(string triggerId, BaseUnit target, IProjectile projectile)
    //{
    //    ApplyTriggerEffect(triggerId, target,projectile.Source);
    //}

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
        _configs = Extensions.GetAssetsFromPath<BaseStatTriggerConfig>(Constants.Configs.c_TriggersConfigsPath,true);
        _projectileCfgs = Extensions.GetAssetsFromPath<ProjectileDataConfig>(Constants.Configs.c_ProjectileConfigsPath);
    }

}

