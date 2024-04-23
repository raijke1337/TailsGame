using Arcatech.Triggers;
using System;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class StunsController : BaseController, IStatsComponentForHandler, ITakesTriggers
    {
        public event SimpleEventsHandler StunHappenedEvent;
        private StatValueContainer _stunCont;
        private float _graceTime;
        private float _regen;
        [SerializeField] private bool isGracePeriod = false;
        private Timer _timer;


        public StunsController(BaseUnit u, string ID = "default") : base(u)
        {
            var cfg = DataManager.Instance.GetConfigByID<StunsControllerConfig>(ID);
            if (cfg == null) return;
            IsReady = true;
            _stunCont = new StatValueContainer(cfg.StunResistance);
            _regen = cfg.RegenPerSec;
            _graceTime = cfg.GracePeriod;
        }
        public override void UpdateInDelta(float deltaTime)
        {
            base.UpdateInDelta(deltaTime);
            if (_timer == null) return; //TODO something is werid here

            if (_stunCont.GetCurrent <= 0f && !isGracePeriod)
            {
                StunHappenedEvent?.Invoke();
                isGracePeriod = true;
                _timer.ResetTimer();
            }
            _timer.TimerTick(deltaTime);
            _stunCont.ChangeCurrent(deltaTime * _regen);

        }
        public override string GetUIText
        {
            get
            {
                if (_stunCont.GetCurrent == _stunCont.GetMax || isGracePeriod)
                {
                    return ("Bonked!");
                }
                else
                {
                    return ($"{_stunCont.GetMax - _stunCont.GetCurrent}");
                }
            }
        }


        public override void SetupStatsComponent()
        {
            _stunCont.Setup();
            _timer = new Timer(_graceTime);
            _timer.TimeUp += OnTimerExpiry;
        }
        private void OnTimerExpiry(Timer timer)
        {
            isGracePeriod = false;
        }
        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            return _stunCont;
        }
    }
}