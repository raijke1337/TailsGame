using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.Managers
{
    public class TriggersProjectilesManager : LoadedManagerBase
    {
        // transform, destroy
        private List<IProjectile> _projectiles = new List<IProjectile>();
        private Dictionary<string, BaseStatTriggerConfig> _triggersD = new Dictionary<string, BaseStatTriggerConfig>();
        private Dictionary<string, ProjectileDataConfig> _projectilesD = new Dictionary<string, ProjectileDataConfig>();

        private List<BaseTrigger> _allTriggers = new List<BaseTrigger>();

        private List<BaseStatTriggerConfig> _list = new List<BaseStatTriggerConfig>();


        #region LoadedManager
        public override void Initiate()
        {
            UpdateDatas();
            var baseTriggers = FindObjectsOfType<BaseTrigger>(); // find triggers on level

            foreach (var t in baseTriggers)
            {
                // weapons are registered separately by controller but there is a check to prevent repeat addition
                RegisterTrigger(t);
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
            foreach (var t in _allTriggers)
            {
                t.TriggerApplicationRequestEvent -= ApplyTriggerEffect;
            }

            var _skillsMan = GameManager.Instance.GetGameControllers.SkillsPlacerManager;
            _skillsMan.ProjectileSkillCreatedEvent -= NewProjectile;
            _skillsMan.SkillAreaPlacedEvent -= RegisterTrigger;
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
            if (!(source is PlayerUnit) && !(source is NPCUnit)) // traps , could keep null maybe? dont like it
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
                    default:
                        Debug.LogError($"Trigger {ID} from {source.GetID} to {target.GetID} did not apply!");
                        break;
                }
            }
            if (finaltgt == null)
            {
                return;
            }
            if (config.Effects.Effects.TryGetValue(EffectMoment.OnCollision, out var p))
            {
                Instantiate(p, finaltgt.transform.position, Quaternion.identity);
            }            
            if (config.Effects.Sounds.TryGetValue(EffectMoment.OnCollision, out var s))
            {
                EffectsManager.Instance.PlaySound(s, finaltgt.transform.position);
            }

            finaltgt.AddTriggeredEffect(effect);
        }

        private void RegisterTrigger(IAppliesTriggers item)
        {
            item.TriggerApplicationRequestEvent += ApplyTriggerEffect;
        }

        public void RegisterRangedWeapon(RangedWeapon weap)
        {
            weap.PlacedProjectileEvent += NewProjectile;
        }
        public void RegisterTrigger(BaseTrigger t)
        {
            if (_allTriggers.Contains(t)) return;
            t.TriggerApplicationRequestEvent += ApplyTriggerEffect;
            _allTriggers.Add(t);
        }

        private void NewProjectile(IProjectile proj)
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

}