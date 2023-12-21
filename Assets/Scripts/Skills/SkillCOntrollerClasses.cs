using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Arcatech.Skills
{ 
    #region skill configs

    [Serializable]
    public class SkillObjectForControls
    {

        public BaseUnit Owner { get; }
        public TextContainerSO Description { get; }


        public float FullCooldown { get; }
        public float CurrentCooldown { get; protected set; }
        public int Cost { get; }


        public SkillArea AreaOfEffectPrefab { get; }
        public SphereSettings AreaSettings { get; }
        public SkillPlacer PlacerPrefab { get; }
        public SphereSettings PlacerSettings { get; }


        public BaseStatTriggerConfig[] Triggers { get; }
         public EffectsCollection Effects { get; }


        public ProjectileConfiguration GetProjectileData
        { get;private set;}

        public InstantiatedSkillObjects SkillObjects;

        public SkillObjectForControls(SkillControlSettingsSO cfg,BaseUnit owner)
        {

            //ID = cfg.ID;
            FullCooldown = cfg.Cooldown;
            Cost = cfg.Cost;
            Effects = cfg.Effects;
            Triggers = new BaseStatTriggerConfig[cfg.Triggers.Length];
            for (int i = 0; i < cfg.Triggers.Length; i++)
            {
                Triggers[i] = cfg.Triggers[i];
            }

            if (cfg is DodgeSkillConfiguration dodge)
            {
                Debug.Log($"TODO dodge skill setting {dodge}");
            }
            if (cfg is ProjectileSkillSO projectile)
            {
                GetProjectileData = projectile.SkillProjectile;
            }
            AreaOfEffectPrefab = cfg.AreaOfEffect;
            AreaSettings = cfg.AreaSettings;
            PlacerPrefab = cfg.Placer;
            PlacerSettings = cfg.PlacerSettings;

            CurrentCooldown = FullCooldown;

            Owner = owner;
            SkillObjects = new InstantiatedSkillObjects();
            SkillObjects.Placer.TriggerEnterEvent += PlacerTriggerEvent; //null here

            SkillObjects.Area.TriggerEnterEvent += AreaTriggerEvent;
        }

        public event SimpleEventsHandler<Collider> PlacerTriggerEvent;
        public event SimpleEventsHandler<Collider> AreaTriggerEvent;


        public virtual bool TryUse()
        {
            bool ok = CurrentCooldown > FullCooldown;
            if (ok)
            {
                CurrentCooldown = 0;
            }

            return ok;
        }
        public virtual float UpdateInDelta(float time)
        {
            return CurrentCooldown += time;
        }
    }


    #endregion
    [Serializable]
    public class SphereSettings
    {
        [Range(0.1f,10f)]public float Radius;
        [Range(0.01f, 10f)] public float TotalTime;     
    }

    public class InstantiatedSkillObjects
    {
        public SkillPlacer Placer;
        public SkillArea Area;
    }
}
