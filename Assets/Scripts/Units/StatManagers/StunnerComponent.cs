using System;
using UnityEngine;

namespace Assets.Scripts.Units
{
    [Serializable]
    public class StunsController : BaseController, IStatsComponentForHandler, ITakesTriggers
    {
        public event SimpleEventsHandler StunHappenedEvent;
        private StatValueContainer CurrentValue;
        private float _graceTime;
        private float _regen;
        [SerializeField] private bool isGracePeriod = false;
        private Timer _timer;

#if UNITY_EDITOR
        [SerializeField] private float _currValue;
#endif


        public StunsController(string ID = "default")
        {
            var cfg = DataManager.Instance.GetConfigByID<StunsControllerConfig>(ID);
            if (cfg == null) return;
            IsReady = true;
            CurrentValue = new StatValueContainer(cfg.StunResistance);
            _regen = cfg.RegenPerSec;
            _graceTime = cfg.GracePeriod;
        }
        public override void UpdateInDelta(float deltaTime)
        {
            base.UpdateInDelta(deltaTime);
            if (CurrentValue.GetCurrent <= 0f && !isGracePeriod)
            {
                StunHappenedEvent?.Invoke();
                isGracePeriod = true;
                _timer.ResetTimer();
            }
            _timer.TimerTick(deltaTime);
            CurrentValue.ChangeCurrent(deltaTime * _regen);
#if UNITY_EDITOR
            _currValue = CurrentValue.GetCurrent;
#endif
        }

        public override void SetupStatsComponent()
        {
            CurrentValue.Setup();
            _timer = new Timer(_graceTime);
            _timer.TimeUp += OnTimerExpiry;
        }
        private void OnTimerExpiry(Timer timer)
        {
            isGracePeriod = false;
        }
        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            return CurrentValue;
        }
    }
}