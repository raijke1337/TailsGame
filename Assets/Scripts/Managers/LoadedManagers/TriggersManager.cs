using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.Managers
{
    public class TriggersManager : LoadedManagerBase
    {
        private List<BaseLevelEventTrigger> _staticT = new List<BaseLevelEventTrigger>();
        //private List<BaseStatTriggerConfig> _list = new List<BaseStatTriggerConfig>();


        protected EffectsManager _effects;




        #region LoadedManager
        public override void Initiate()
        {
            //UpdateDatas();
            var staticTriggers = FindObjectsOfType<BaseLevelEventTrigger>(); // find triggers on level

            foreach (var t in staticTriggers)
            {
                t.TriggerHitUnitEvent += (ta, tb) => HandleStaticTrigger(ta, tb, t);
                _staticT.Add(t);
            }
            _effects = EffectsManager.Instance;

        }


        public override void RunUpdate(float delta)
        {
            if (_projectiles == null) return;
            foreach (var p in _projectiles.ToList())
            {
                p.UpdateInDelta(delta);
            }
        }

        public override void Stop()
        {
            foreach (var t in _staticT)
            {
                t.TriggerHitUnitEvent -= (ta, tb) => HandleStaticTrigger(ta, tb, t);
            }

            var _skillsMan = GameManager.Instance.GetGameControllers.SkillsPlacerManager;
            //_skillsMan.ProjectileSkillCreatedEvent -= NewProjectile;
            // _skillsMan.SkillAreaPlacedEvent -= RegisterTrigger;
        }
        #endregion

        #region triggers

        private void HandleStaticTrigger(BaseUnit target, bool isEnter, BaseLevelEventTrigger lv)
        {
           // Debug.Log($"{this} received a call from {lv} : {target} is entering the zone: {isEnter}");
        }

        public void ServeTriggerApplication(BaseStatTriggerConfig cfg, BaseUnit source, BaseUnit target, bool IsEnter)
        {
            //Debug.Log($"Applying trigger {cfg.ID} from {source} to {target}");
            if (IsEnter) ApplyTriggerEffect(cfg, target, source);
        }


        private void ApplyTriggerEffect(BaseStatTriggerConfig config, BaseUnit target, BaseUnit source)
        {
            TriggeredEffect effect = new TriggeredEffect(config);
            BaseUnit finaltgt = null;

            if (source is null)
            {
                // static trigger (trap, health pack etc)
                switch (config.TargetType)
                {
                    case TriggerTargetType.TargetsEnemies:
                        if (target is PlayerUnit player)
                        {
                            finaltgt = player;
                        }
                        break;
                    case TriggerTargetType.TargetsUser:
                        break;
                    case TriggerTargetType.TargetsAllies:
                        break;
                }
            }


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
                        //Debug.LogError($"Trigger {config} from {source.GetID} to {target.GetID} did not apply!");
                        break;
                }
            }
            if (finaltgt == null)
            {
                return;
            }

            _effects.ServeEffectsRequest(new EffectRequestPackage(config.Effects, EffectMoment.OnCollision, target.transform));
            finaltgt.PickTriggeredEffectHandler(effect);

        }

        // statics are found manually an others are served from unit manager

        #endregion



        #region projectiles

        private List<ProjectileComponent> _projectiles = new List<ProjectileComponent>();

        public void ServeProjectileRequest(ProjectileComponent proj, BaseUnit owner) // spawn from weapon, setup done by weapon
        {
            if (_projectiles == null)
            {
                _projectiles = new List<ProjectileComponent>();
            }
            proj.transform.SetPositionAndRotation(owner.GetEmpties.ItemPositions[EquipItemType.RangedWeap].transform.position,
                owner.GetEmpties.ItemPositions[EquipItemType.RangedWeap].transform.rotation);

            proj.transform.forward = owner.transform.forward;

            _projectiles.Add(proj);
            ProjectileSubs(proj);
        }
        public ProjectileComponent ServeProjectileRequest(ProjectileConfiguration proj, BaseUnit owner) // spawn from skills manager , needed to set the location correctly
        {
            var p = proj.GetProjectile(owner);
            ServeProjectileRequest(p, owner);   
            return p;
        }


        private void ProjectileSubs(ProjectileComponent comp)
        {
            comp.ProjectileEnteredTriggerEvent += ProjectileHit;
            comp.ProjectileExpiredEvent += ProjectileExpired;
        }

        private void ProjectileExpired(ProjectileComponent arg)
        {
            arg.ProjectileEnteredTriggerEvent -= ProjectileHit;
            arg.ProjectileExpiredEvent -= ProjectileExpired;
            _projectiles.Remove(arg);
        }

        private void ProjectileHit(Collider col, ProjectileComponent proj)
        {
            if (col.gameObject.isStatic)
            {
                proj.StopProjectile(col.transform);
                return;
            }
            if (col.TryGetComponent(out BaseUnit hit) && hit.Side != proj.Owner.Side)
            {
                if (proj.Decrement(1) <= 0)
                {
                    proj.StopProjectile(col.transform);
                }
            }
        }
        #endregion
    }

}