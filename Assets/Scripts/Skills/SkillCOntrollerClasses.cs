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
    public class SkillObjectForControls
    {
        public SkillObjectForControls(SkillControlSettingsSO cfg, BaseUnit owner)
        {
            Owner = owner;
            _settings = cfg;
            _cdTimers = new Queue<Timer>();
        }
        public virtual void UpdateInDelta(float time)
        {
            foreach (var timer in _cdTimers.ToList())
            {
                timer.TimerTick(time);
            }
        }

        #region public
        public BaseUnit Owner { get; }
        public ExtendedTextContainerSO Description { get => _settings.Description; }
        public BaseStatTriggerConfig[] Triggers { get => _settings.Triggers; }
        public EffectsCollection Effects { get => _settings.Effects; }

        public float PlacerRadius { get => _settings.PlacerRadius; }
        public float EffectRadius { get => _settings.AoERadius; }
        // end


        public bool TryUseSkill (out TriggeredEffect cost)
        {
            cost = new TriggeredEffect(_settings.ComboCostTrigger);
            return (_cdTimers.Count < _settings.Charges);
        }
        public virtual SkillComponent ProduceSkillObject { get
        {
                UsedSkillTImer();
            var skillGameobject = GameObject.Instantiate(_settings.SkillObject);
            skillGameobject.Data = _settings;
            if (skillGameobject is BoosterSkillInstanceComponent bb)
            {
                bb.Data = _settings as DodgeSkillConfigurationSO;
            }

            skillGameobject.Owner = Owner;
            return skillGameobject;
        }

        }
        #endregion
        #region cd
        // from stored cfg
        private readonly SkillControlSettingsSO _settings;
        private Queue<Timer> _cdTimers;
        private void UsedSkillTImer()
        {
            var t = new Timer(_settings.ChargeRestore);
            _cdTimers.Enqueue(t);
            t.TimeUp += T_TimeUp;
        }

        private void T_TimeUp(Timer arg)
        {
            _cdTimers.Dequeue();
            arg.TimeUp -= T_TimeUp;
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
    public class ProjectileSkillObjectForControls : SkillObjectForControls
    {
        public ProjectileConfiguration GetProjectileData
        { get; }
        public ProjectileSkillObjectForControls(ProjectileSkillConfigurationSO cfg, BaseUnit owner) : base(cfg, owner)
        {
            GetProjectileData = cfg.SkillProjectile;
        }
    }



}
