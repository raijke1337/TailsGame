using Arcatech.Effects;
using Arcatech.Triggers;
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
    public class SkillAoESettings
    {
        public float GrowTime { get; }
        public Vector3 StartRad { get; }
        public Vector3 EndRad { get; }

        public SkillAoESettings(float time, Vector3 start, Vector3 end)
        {
            GrowTime = time;
            StartRad = start;
            EndRad = end;
        }
        public SkillAoESettings(float time)
        {
            GrowTime = time;
            StartRad = EndRad = Vector3.one;
        }
    }
    [Serializable]
    public class SkillObjectForControls
    {
        private Timer _recTimer;
        private bool _isReady = true;
        private float _baseCd;

        public float GetCD => _recTimer.GetRemaining;
        public string ID { get; }
        public SkillAoESettings AoESettings { get; }
        public BaseStatTriggerConfig[] GetTriggers { get; }

        public BaseSkill Prefab;
        public int Cost { get;private set; }
        public EffectsCollection Effects { get; }

        public SkillObjectForControls(SkillControlSettingsSO cfg)
        {

            ID = cfg.ID;
            _baseCd = cfg.Cooldown;
            Prefab = cfg.Prefab;
            Cost = cfg.Cost;
            Effects = cfg.Effects;
            Vector3 rad = new(cfg.StartRad, cfg.StartRad);
            Vector3 endR = new(cfg.EndRad, cfg.EndRad);
            AoESettings = new SkillAoESettings(cfg.GrowTime, rad, endR);
            GetTriggers = new BaseStatTriggerConfig[cfg.Triggers.Length];
            for (int i = 0; i < cfg.Triggers.Length; i++)
            {
                GetTriggers[i] = cfg.Triggers[i];
            }

            _recTimer = new Timer(_baseCd);
            _recTimer.TimeUp += _recTimer_TimeUp;
        }


        public virtual bool TryUse()
        {
            bool result = _isReady;
            if (_isReady)
            {
                _recTimer.ResetTimer();
                _isReady = false;
            }
            return result;
        }
        public virtual float UpdateInDelta(float time)
        {
            return _recTimer.TimerTick(time);
        }
        private void _recTimer_TimeUp(Timer arg)
        {
            _isReady = true;
        }
    }

    #endregion
}
