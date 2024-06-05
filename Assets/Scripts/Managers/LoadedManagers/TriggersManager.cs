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

        protected EffectsManager _effects;


        #region LoadedManager
        public override void Initiate()
        {
            //UpdateDatas();
            var staticTriggers = FindObjectsOfType<BaseLevelEventTrigger>(); // find triggers on level
            if (ShowDebug) Debug.Log($"Found {staticTriggers.Length} triggers on level");

            foreach (var t in staticTriggers)
            {

                //Debug.Log($"Add {t.name}");
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
        }
        #endregion

        #region triggers

        private void HandleStaticTrigger(BaseUnit target, bool isEnter, BaseLevelEventTrigger lv)
        {
            if (lv is LevelEffectTrigger eff && target is PlayerUnit) // case : trap or health up
            {
                foreach (var config in eff.Triggers)
                {
                    ServeTriggerApplication(new TriggeredEffect (config),null, target, isEnter);
                }            
            }
        }


        // here we need a check to prevent repeat applications so individual instances are accepted instead

        public void ServeTriggerApplication(TriggeredEffect triggerEffect, BaseUnit source, BaseUnit target, bool IsEnter)
        {
            if (IsEnter) ApplyTriggerEffect(triggerEffect, target, source);
        }

        private Dictionary<TriggeredEffect, List<BaseUnit>> _hitTargetsPerEffect = new Dictionary<TriggeredEffect, List<BaseUnit>>();
        private void ApplyTriggerEffect(TriggeredEffect effect, BaseUnit target, BaseUnit source)
        {

            BaseUnit finaltgt = null;

            if (source is null)
            {
                // static trigger (trap, health pack etc)
                switch (effect.Target)
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
                switch (effect.Target)
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
                switch (effect.Target)
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
                switch (effect.Target)
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

            // new part to check for repeat applications 
            if (_hitTargetsPerEffect.TryGetValue(effect, out var hit))
            {
                if (hit.Contains(finaltgt))
                {
                    //Debug.Log($"trigger {effect.ID} already applied to {finaltgt.name}, skipping");
                    return; // 
                }
                else
                {
                    hit.Add(finaltgt);                    
                    finaltgt.ApplyEffectToController(effect);
                    if (ShowDebug) { Debug.Log($"trigger {effect.ID} applied to {finaltgt.name}"); }

                    HandleEffectsFromTrigger(effect, EffectMoment.OnCollision, finaltgt.transform);
                }
            }
            else
            {
                _hitTargetsPerEffect[effect] = new List<BaseUnit>();
                _hitTargetsPerEffect[effect].Add(finaltgt); 
                finaltgt.ApplyEffectToController(effect);
                if (ShowDebug) { Debug.Log($"trigger {effect.ID} applied to {finaltgt.name}"); }

                HandleEffectsFromTrigger(effect, EffectMoment.OnCollision, finaltgt.transform);
            }
        }

        // statics are found manually an others are served from unit manager

        #endregion

        #region forward to effects mng
        private void HandleEffectsFromTrigger (TriggeredEffect e, EffectMoment when, Transform where)
        {
            var pack = new EffectRequestPackage(e.GetEffects.GetRandomSound(when), e.GetEffects.GetRandomEffect(when), null, where);
            _effects.ServeEffectsRequest(pack);
        }

        #endregion


        #region projectiles

        private List<ProjectileComponent> _projectiles = new List<ProjectileComponent>();


        public void RegisterExistingProjectile (ProjectileComponent p)
        {
            if (_projectiles == null)
            {
                _projectiles = new List<ProjectileComponent>();
            }
            _projectiles.Add(p);

            p.ProjectileEnteredTriggerEvent += ProjectileHit;
            p.ProjectileExpiredEvent += ProjectileExpired;

            PositionProjectile(p);



            //Debug.Log($"{p} was registered and is set up properly: {p.IsSetup}");
        }

        private void PositionProjectile (ProjectileComponent spawned)
        {
            var Owner = spawned.Owner;

            spawned.transform.SetPositionAndRotation(Owner.GetEmpties.ItemPositions[spawned.GetProjectileSettings.SpawnPlace].transform.position,
                Owner.GetEmpties.ItemPositions[spawned.GetProjectileSettings.SpawnPlace].transform.rotation);

            spawned.transform.forward = Owner.transform.forward;
        }



        private void ProjectileExpired(ProjectileComponent arg)
        {
           // Debug.Log($"{arg} reported expiry");
            arg.ProjectileEnteredTriggerEvent -= ProjectileHit;
            arg.ProjectileExpiredEvent -= ProjectileExpired;
            _projectiles.Remove(arg);
        }

        private void ProjectileHit(Collider col, ProjectileComponent proj)
        {
          //  Debug.Log($"Projectile {proj} hit {col}");
            if (col.gameObject.CompareTag("StaticItem"))
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