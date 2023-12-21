using Arcatech.Effects;
using Arcatech.Skills;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Managers
{
    public class SkillsPlacerManager : LoadedManagerBase
    {


        private EffectsManager effectsManager;
        private TriggersManager triggers;

        private List<SkillObjectForControls> _cachedSkills;

        #region ManagerBase
        public override void Initiate()
        {
            effectsManager = EffectsManager.Instance;
            triggers = GameManager.Instance.GetGameControllers.TriggersProjectilesManager;
            _cachedSkills = new List<SkillObjectForControls>();
        }

        public override void RunUpdate(float delta)
        {
            foreach (SkillObjectForControls skillObject in _cachedSkills)
            {
             if (skillObject.SkillObjects.Placer!= null) skillObject.SkillObjects.Placer.UpdateInDelta(delta);
              if (skillObject.SkillObjects.Area!= null) skillObject.SkillObjects.Area.UpdateInDelta(delta);
            }
        }

        public override void Stop()
        {

        }

        #endregion


        public void ServeSkillRequest(SkillObjectForControls data, BaseUnit source, Transform where)
        {
            _cachedSkills.Add(data);

            var placer = data.SkillObjects.Placer = Instantiate(data.PlacerPrefab,transform);

            effectsManager.ServeEffectsRequest(new EffectRequestPackage(data.Effects, EffectMoment.OnStart, where));

            placer.SetupStatsComponent();
            placer.transform.SetPositionAndRotation(where.position, source.transform.rotation) ;
            placer.TimeToLive = data.PlacerSettings.TotalTime;
            if (placer.Collider is SphereCollider sp)
            {
                sp.radius = data.PlacerSettings.Radius;
            }

            if (data.GetProjectileData!=null)
            {
                placer.TimeToLive = data.GetProjectileData.Settings.TimeToLive;
                triggers.ServeProjectileRequest(data.GetProjectileData, source); // proejctile skill
            }
            data.PlacerTriggerEvent += (t) => PlacerHit(t,data);
            data.AreaTriggerEvent += (t) => AreaHit(t, data);

        }


        private void PlacerHit(Collider hit, SkillObjectForControls comp)
        {
            if (CheckAreaCollision(hit,comp,out var p))
            {
                Destroy(comp.SkillObjects.Placer);
                comp.SkillObjects.Area = Instantiate(comp.AreaOfEffectPrefab, p.position, p.rotation);

                comp.SkillObjects.Area.TimeToLive = comp.AreaSettings.TotalTime;
                if (comp.SkillObjects.Area.TryGetComponent(out SphereCollider sp))
                {
                    sp.radius = comp.AreaSettings.Radius;
                }
            }
        }
        private void AreaHit(Collider hit, SkillObjectForControls comp)
        {
            if (hit.TryGetComponent(out BaseUnit u))
            {
                Debug.Log($"Skill area of {comp.Description.Title} hit {u}");
                foreach (var t in comp.Triggers)
                {
                    triggers.ServeTriggerApplication(t, comp.Owner, u, true);
                }
            }
        }



        private bool CheckAreaCollision(Collider hit, SkillObjectForControls comp, out Transform where)
        {
            where = null;

            if (hit.gameObject.isStatic || (hit.TryGetComponent(out BaseUnit unit) && comp.Owner != unit))
            {
                where = hit.transform;
                return true;
            }

            else return false;
        }
    
    }

}