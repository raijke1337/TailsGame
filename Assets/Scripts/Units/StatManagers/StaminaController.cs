using Arcatech.Managers;
using Arcatech.Triggers;
using System;
using TMPro;

namespace Arcatech.Units
{
    [Serializable]
    public class StaminaController : BaseController, IStatsComponentForHandler, ITakesTriggers
    {

        private StatValueContainer _container;

        protected float Degen;
        protected float Timeout;
        private float _currentTimeout = 0f;

        public StaminaController(BaseUnit owner) : base(owner)
        {

            var cfg = DataManager.Instance.GetConfigByID<ComboStatsConfig>("default");
            if (cfg == null)
            {
                IsReady = false;
                return;
            }
            _container = new StatValueContainer(cfg.ComboContainer);
            Degen = cfg.DegenCoeff;
            Timeout = cfg.HeatTimeout;
            _container.Setup();

            IsReady = true;
        }

        #region gameplay

        #endregion
        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            return _container;
        }
        #region UI
        public override string GetUIText { get => ($"{_container.GetCurrent} / {_container.GetMax}"); }
        public StatValueContainer GetComboContainer => _container;

        #endregion



        #region managed

        public override void UpdateInDelta(float deltaTime)
        {

            base.UpdateInDelta(deltaTime);
            if (_currentTimeout <= Timeout)
            {
                _currentTimeout += deltaTime;
                return;
            }
            _container.ChangeCurrent(-Degen * deltaTime);
        }

        #endregion

    }

}