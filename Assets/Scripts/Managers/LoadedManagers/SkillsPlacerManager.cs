using Arcatech.Skills;
using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Managers
{
    public class SkillsPlacerManager : LoadedManagerBase
    {
        public event SimpleEventsHandler<IProjectile> ProjectileSkillCreatedEvent;
        public event SimpleEventsHandler<IAppliesTriggers> SkillAreaPlacedEvent;

        private EffectsManager _effects;

        #region ManagerBase
        public override void Initiate()
        {
            GameManager.Instance.GetGameControllers.UnitsManager.UnitRequestsToPlaceASkillEvent += DoSkillRequest;
            _effects = EffectsManager.Instance;
            //LoadBaseSkills();
            //LoadDatasIntoSkills();
        }

        public override void RunUpdate(float delta)
        {

        }

        public override void Stop()
        {
            GameManager.Instance.GetGameControllers.UnitsManager.UnitRequestsToPlaceASkillEvent -= DoSkillRequest;
        }

        #endregion


        private void DoSkillRequest(SkillObjectForControls data, BaseUnit source, Transform where)
        {
            var skill = Instantiate(data.Prefab);

            skill.Owner = source;
            skill.AssignValues(data);
            skill.SetupStatsComponent();
           
            skill.transform.SetPositionAndRotation(where.position, where.rotation);

            _effects.PlaceParticle(data.Effects.Effects[EffectMoment.OnStart],source.transform);
            _effects.PlaySound(data.Effects.Sounds[EffectMoment.OnStart], source.transform.position);

            if (skill is IProjectile)
            {
                var s = skill as IProjectile;
                ProjectileSkillCreatedEvent?.Invoke(s);
                // further handling by projectiles manager (expiry, movement)
            }
            else
            {
                SkillAreaPlacedEvent?.Invoke(skill);
                skill.HasExpiredEvent += HandleSkillExpiry;
            }
        }


        private void HandleSkillExpiry(IExpires item)
        {
            item.HasExpiredEvent -= HandleSkillExpiry;
            //AudioManager.Instance.UnRegisterSound(this as IHasSounds);
            Destroy(item.GetObject());
        }


    }

}