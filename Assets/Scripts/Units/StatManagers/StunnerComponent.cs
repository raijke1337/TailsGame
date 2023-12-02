using System;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class StunsController : BaseController, IStatsComponentForHandler, ITakesTriggers
    {
        public event SimpleEventsHandler StunHappenedEvent;
        private StatValueContainer _current;
        private float _graceTime;
        private float _regen;
        [SerializeField] private bool isGracePeriod = false;
        private Timer _timer;

#if UNITY_EDITOR
        [SerializeField] private float _currValue;
#endif


        public StunsController(BaseUnit u, string ID = "default") : base (u)
        {
            var cfg = DataManager.Instance.GetConfigByID<StunsControllerConfig>(ID);
            if (cfg == null) return;
            IsReady = true;
            _current = new StatValueContainer(cfg.StunResistance);
            _regen = cfg.RegenPerSec;
            _graceTime = cfg.GracePeriod;
        }
        public override void UpdateInDelta(float deltaTime)
        {
            base.UpdateInDelta(deltaTime);
            if (_timer == null) return; //TODO something is werid here

            if (_current.GetCurrent <= 0f && !isGracePeriod)
            {
                StunHappenedEvent?.Invoke();
                isGracePeriod = true;
                _timer.ResetTimer();
            }
            _timer.TimerTick(deltaTime);
            _current.ChangeCurrent(deltaTime * _regen);
#if UNITY_EDITOR
            _currValue = _current.GetCurrent;
#endif
        }

        public override void SetupStatsComponent()
        {
            _current.Setup();
            _timer = new Timer(_graceTime);
            _timer.TimeUp += OnTimerExpiry;
        }
        private void OnTimerExpiry(Timer timer)
        {
            isGracePeriod = false;
        }
        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            return _current;
        }
    }
}