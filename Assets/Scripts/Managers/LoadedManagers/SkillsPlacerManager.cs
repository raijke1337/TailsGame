using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Units;
using System.Linq;
using UnityEngine;
namespace Arcatech.Managers
{
    public class SkillsPlacerManager : LoadedManagerBase
    {


        private EffectsManager effectsManager;
        private TriggersManager triggers;

        #region ManagerBase
        public override void Initiate()
        {
            effectsManager = EffectsManager.Instance;
            triggers = GameManager.Instance.GetGameControllers.TriggersProjectilesManager;
        }

        public override void RunUpdate(float delta)
        {

        }

        public override void Stop()
        {

        }

        #endregion


        public void ServeSkillRequest(SkillProjectileComponent comp, BaseUnit source, Transform where)
        {
            if (ShowDebug)
            {
                Debug.Log($"Request for {comp.name} from {comp.Owner} at {where.position}");
            }
            // receive the same proj as projectiles manager but do the specific handling

            effectsManager.ServeEffectsRequest(new EffectRequestPackage
                (comp.GetEffects.GetRandomSound(EffectMoment.OnStart),
                comp.GetEffects.GetRandomEffect(EffectMoment.OnStart),
                null, where));

            if (comp is BoosterSkillInstanceComponent bs)
            {
                source.UnitDodge(bs);
            }
            


            // here we already get the projectile gameobject with everything set up...


            comp.TriggerEnterEvent += (t, t2) => HandleSkillTriggerEvent(t, t2, comp);
            comp.SkillDestroyedEvent += HandleSkillDestructionEvent;
            if (ShowDebug)
            {

                Debug.Log($"Register skill {comp.name} from {comp.Owner}");
            }
        }

        private void HandleSkillTriggerEvent(Collider col, SkillState state, SkillProjectileComponent comp)
        {
            if (CheckAreaCollision(col, comp, out var w, out var t) && t != null)
            {
                if (ShowDebug)
                {
                    Debug.Log($"Skill {comp.name} from {comp.Owner} tirgger event:\n{state}, by {col}");
                }

                switch (state)
                {
                    case SkillState.Placer:
                        effectsManager.ServeEffectsRequest(new EffectRequestPackage
    (comp.GetEffects.GetRandomSound(EffectMoment.OnCollision),
    comp.GetEffects.GetRandomEffect(EffectMoment.OnCollision),
    null, col.transform));

                        comp.AdvanceStage();
                        break;
                    case SkillState.AoE:
                        foreach (var ef in comp.GetEffectCfgs)
                        {
                            triggers.ServeTriggerApplication(new Triggers.StatsEffect(ef), comp.Owner, t, true);
                        }

                        //if (t!=null) Debug.Log($"{comp.Data.Description.Title} has hit {t}");
                        break;
                }
            }
        }
        private void HandleSkillDestructionEvent(SkillProjectileComponent c)
        {
            if (ShowDebug)
            {
                Debug.Log($"Unegister skill {c.name} from {c.Owner}");
            }
            c.SkillDestroyedEvent -= HandleSkillDestructionEvent;
            c.TriggerEnterEvent -= (t, t2) => HandleSkillTriggerEvent(t, t2, c);
        }


        private bool CheckAreaCollision(Collider hit, SkillProjectileComponent comp, out Transform where, out BaseUnit taget)
        {
            where = null;
            taget = null;

            if ((comp is BoosterSkillInstanceComponent d) || hit.gameObject.CompareTag("SolidItem") || //hits a wall
                (hit.TryGetComponent(out taget) && comp.Owner != taget) || // enemy target skills
                (comp.GetEffectCfgs.First().TargetType == TriggerTargetType.TargetsUser && taget == comp.Owner)) // self target skills
            {
                where = hit.transform;
                return true;
            }

            else return false;
        }

    }

}