using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Arcatech.Skills
{

    [Serializable]
    public class SkillObjectForControls : ICost
    {
        public SkillObjectForControls(SerizalizedSkillConfiguration cfg, BaseUnit owner)
        {
            Owner = owner;
            _settings = cfg;
            _cdTimers = new Queue<Timer>();
            Transform p = null;
            owner.GetEmpties.ItemPositions.TryGetValue(cfg.SpawnPlace,out p);

            _effects = new EffectsCollection(cfg.Effects, p);
            _cost = cfg.CostTrigger;
        }
        public virtual void UpdateInDelta(float time)
        {
            foreach (var timer in _cdTimers.ToList())
            {
                timer.Tick(time);
            }
        }

        #region public
        public BaseUnit Owner { get; }
        public ExtendedText Description { get => _settings.Description; }
        public SerializedStatTriggerConfig[] Triggers { get => _settings.Triggers; }

        private EffectsCollection _effects;
        public EffectsCollection Effects { get => _effects; }

        SerializedStatTriggerConfig _cost { get; }

        public float PlacerRadius { get => _settings.PlacerRadius; }
        public float EffectRadius { get => _settings.AoERadius; }
        // end




        public bool SkillCooldownReady
        {
            get
            {
                return (_cdTimers.Count < _settings.Charges);
            }
        }
        public virtual SkillProjectileComponent UseSkill { get
        {

                UsedSkillTimer();
            var skillGameobject = GameObject.Instantiate(_settings.SkillProjectileConfig.ProjectilePrefab) as SkillProjectileComponent;

                skillGameobject.Setup(_settings,Owner);


            if (skillGameobject is BoosterSkillInstanceComponent bb)
            {
                bb.InitializeDodgeSettings((_settings as DodgeSkillConfigurationSO));
            }


            return skillGameobject;
        }

        }
        #endregion
        #region cd
        // from stored cfg
        private readonly SerizalizedSkillConfiguration _settings;
        private Queue<Timer> _cdTimers;
        private void UsedSkillTimer()
        {
            var t = new CountDownTimer(_settings.ChargeRestore);
            _cdTimers.Enqueue(t);
            t.OnTimerStopped += () => _cdTimers.Dequeue();
        }
 
        #endregion

        #region UI

        public string GetTextForUI
        {
            get
            {
                return (_settings.Charges - _cdTimers.Count).ToString();
            }
        }

        public TriggeredEffect Cost

        {
            get
            {
                return new TriggeredEffect(_cost);
            }
        }

        #endregion
    }

    public class DodgeSkillObjectForControls : SkillObjectForControls
    {

        // this logic was in dodge controller 
        public DodgeSettingsPackage GetDodgeData { get; }
        public DodgeSkillObjectForControls(DodgeSkillConfigurationSO cfg, BaseUnit owner) : base(cfg, owner)
        {
            GetDodgeData = cfg.DodgeSettings;
        }

    }


}
