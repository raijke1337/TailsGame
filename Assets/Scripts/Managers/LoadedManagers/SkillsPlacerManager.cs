using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Units;
using System.Linq;
using UnityEngine;
namespace Arcatech.Managers
{
    public class SkillsPlacerManager : MonoBehaviour, IManagedController
    {


        private EffectsManager effectsManager;
        private TriggersManager triggers;

        #region ManagerBase
        public virtual void StartController()
        {
            effectsManager = EffectsManager.Instance;
            triggers = GameManager.Instance.GetGameControllers.TriggersProjectilesManager;
        }

        public virtual void ControllerUpdate(float delta)
        {

        }
        public virtual void FixedControllerUpdate(float fixedDelta)
        {

        }
        public virtual void StopController()
        {

        }

        #endregion


        public void ServeSkillRequest(SkillProjectileComponent comp, BaseUnit source, Transform where)
        {
            if (comp is BoosterSkillInstanceComponent bs)
            {
                source.UnitDodge(bs);
            }         
                        // here we already get the projectile gameobject with everything set up...


            comp.TriggerEnterEvent += (t, t2) => HandleSkillTriggerEvent(t, t2, comp);
            comp.SkillDestroyedEvent += HandleSkillDestructionEvent;

        }

        private void HandleSkillTriggerEvent(Collider col, SkillState state, SkillProjectileComponent comp)
        {
            if (CheckAreaCollision(col, comp, out var w, out var t) && t != null)
            {


                switch (state)
                {
                    case SkillState.Placer:
                        comp.AdvanceStage();
                        break;
                    case SkillState.AoE:
                        foreach (var ef in comp.GetEffectCfgs)
                        {
                          //  triggers.ServeTriggerApplication(new Triggers.StatsEffect(ef), comp.Owner, t, true);
                        }

                        //if (t!=null) Debug.Log($"{comp.Data.Description.Title} has hit {t}");
                        break;
                }
            }
        }
        private void HandleSkillDestructionEvent(SkillProjectileComponent c)
        {

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