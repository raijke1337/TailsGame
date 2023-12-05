using Arcatech.Triggers;
using System;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class ComboController : BaseController, IStatsComponentForHandler, ITakesTriggers
    {

        private StatValueContainer _container;

        protected float Degen;
        protected float Timeout;
        private float _currentTimeout = 0f;
        public StatValueContainer GetAvailableCombo { get => _container; }


        public ComboController(BaseUnit owner) : base (owner)
        {

            var cfg = DataManager.Instance.GetConfigByID<ComboStatsConfig>(owner.GetID);
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

        public bool UseCombo(float value)
        {
            bool result = _container.GetCurrent >= value;   
            if (result) _container.ChangeCurrent(-value);
            return result;
        }
        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            return _container;
        }

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