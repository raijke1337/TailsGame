using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
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


        public void ServeSkillRequest(SkillComponent comp, BaseUnit source, Transform where)
        {

            effectsManager.ServeEffectsRequest(new EffectRequestPackage(comp.Data.Effects, EffectMoment.OnStart, where));
            if (comp.Data is ProjectileSkillSO pr)
            {
                //placer.TimeToLive = comp.GetProjectileData.Settings.TimeToLive;
                var ppp = triggers.ServeProjectileRequest(pr.SkillProjectile, source); // proejctile skill
                comp.transform.SetParent(ppp.transform, false);
            }
            else
            {
                comp.transform.SetPositionAndRotation(where.position, source.transform.rotation);
            }

            comp.TriggerEnterEvent += (t,t2) => HandleSkillTriggerEvent(t,t2,comp);
            comp.SkillDestroyedEvent += HandleSkillDestructionEvent;

        }

        private void HandleSkillTriggerEvent (Collider col, SkillState state, SkillComponent comp)
        {
            if (CheckAreaCollision(col,comp,out var w,out var t) && t != null)
            {
                switch (state)
                {
                    case SkillState.Placer:
                        effectsManager.ServeEffectsRequest(new EffectRequestPackage(comp.Data.Effects, EffectMoment.OnCollision, w));
                        //Debug.Log($"{comp.Data.Description.Title} activated by {t}");
                        comp.AdvanceStage();
                        break;
                    case SkillState.AoE:
                        foreach (var ef in comp.Data.Triggers)
                        {
                            triggers.ServeTriggerApplication(ef, comp.Owner, t,true);
                        }
                        //if (t!=null) Debug.Log($"{comp.Data.Description.Title} has hit {t}");
                        break;
                }
            }
        }
        private void HandleSkillDestructionEvent(SkillComponent c)
        {
            c.SkillDestroyedEvent -= HandleSkillDestructionEvent;
            c.TriggerEnterEvent -= (t, t2) => HandleSkillTriggerEvent(t, t2, c);
        }


        private bool CheckAreaCollision(Collider hit, SkillComponent comp, out Transform where, out BaseUnit taget)
        {
            where = null;
            taget = null;

            if ((comp.Data is DodgeSkillConfiguration d) || hit.gameObject.isStatic || //hits a wall
                (hit.TryGetComponent(out taget) && comp.Owner != taget) || // enemy target skills
                (comp.Data.Triggers.First().TargetType == TriggerTargetType.TargetsUser &&  taget == comp.Owner)) // self target skills
            {
                where = hit.transform;
                return true;
            }

            else return false;
        }

    }

}