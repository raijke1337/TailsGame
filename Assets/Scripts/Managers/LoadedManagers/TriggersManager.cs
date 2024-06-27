using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.Managers
{
    public class TriggersManager : LoadedManagerBase
    {

        EventBinding<StatsEffectTriggerEvent> _triggersBinding;

        #region LoadedManager
        public override void StartController()
        {
            _triggersBinding = new EventBinding<StatsEffectTriggerEvent>(HandleTriggerEvent);
            _applied = new Dictionary<StatsEffect, List<DummyUnit>>();
            EventBus<StatsEffectTriggerEvent>.Register(_triggersBinding);
        }
        public override void ControllerUpdate(float delta)
        {
            if (_projectiles == null) return;
            foreach (var p in _projectiles.ToList())
            {
                p.UpdateInDelta(delta);
            }
        }
        public override void FixedControllerUpdate(float fixedDelta)
        {

        }

        public override void StopController()
        {
            _applied.Clear();
            EventBus<StatsEffectTriggerEvent>.Deregister(_triggersBinding);
        }
        #endregion

        #region triggers

        private Dictionary<StatsEffect, List<DummyUnit>> _applied;
        private void HandleTriggerEvent(StatsEffectTriggerEvent obj)
        {
            if (DebugMessage)
            {
                Debug.Log($"receive event {obj.AppliedEffects}, to {obj.Target.GetUnitName}");
            }
            // a new set of instances is created for each use of applicatior

            foreach (StatsEffect eff in obj.AppliedEffects)
            {
                if (_applied.TryGetValue(eff, out var r))
                {
                    // effect in list

                    if (r.Contains(obj.Target)) return; // target in list
                    else
                    {
                        // target not in list
                        ApplyTriggerEffect(eff, obj.Target, obj.Source);
                        r.Add(obj.Target);
                    }
                }
                // effect not in list just do normally
                else
                {
                    _applied[eff] = new List<DummyUnit>();
                    _applied[eff].Add(obj.Target);
                    ApplyTriggerEffect(eff, obj.Target, obj.Source);
                }
            }
        }


        private void ApplyTriggerEffect(StatsEffect effect, DummyUnit target, DummyUnit source)
        {
            DummyUnit finaltgt = null;
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
                if (DebugMessage)
                {
                    Debug.Log($"applying effect {effect} to {finaltgt}");
                }
                finaltgt.ApplyEffect(effect);
                return;
            }


            if (finaltgt == null)
            {
                if (DebugMessage)
                {
                    Debug.Log($"fail to apply {effect}");
                }
                return ;
            }
            
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
            //var Owner = spawned.Owner;

            //spawned.transform.SetPositionAndRotation(Owner.GetEmpties.ItemPositions[spawned.GetProjectileSettings.SpawnPlace].transform.position,
            //    Owner.GetEmpties.ItemPositions[spawned.GetProjectileSettings.SpawnPlace].transform.rotation);

            //spawned.transform.forward = Owner.transform.forward;
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
            if (col.gameObject.CompareTag("SolidItem"))
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