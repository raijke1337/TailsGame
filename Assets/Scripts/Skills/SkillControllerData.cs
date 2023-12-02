using System;
namespace Arcatech
{
    [Serializable]
    public class SkillControllerData
    {
        public string ID;
        public CombatActionType SkillType;
        public SkillData GetSkillData { get; }

        private Timer _recTimer;
        private bool _isReady = true;
        public float GetCD => _recTimer.GetRemaining;


        public virtual bool RequestUse()
        {
            bool result = _isReady;
            if (_isReady)
            {
                _recTimer.ResetTimer();
                _isReady = false;
            }
            return result;
        }
        public virtual float Ticks(float time)
        {
            return _recTimer.TimerTick(time);
        }

        public SkillControllerData(SkillControllerDataConfig cfg)
        {
            ID = cfg.ID;
            SkillType = cfg.SkillType;
            GetSkillData = new SkillData(cfg.Data);
            _recTimer = new Timer(GetSkillData.Recharge);
            _recTimer.TimeUp += _recTimer_TimeUp;
        }
        private void _recTimer_TimeUp(Timer arg)
        {
            _isReady = true;
        }
        // all logic moved to SkillsPlacerMan class, here we only have cooldown checking and data

    }

}