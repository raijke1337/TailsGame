using System;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class ComboController : BaseController, IStatsComponentForHandler, ITakesTriggers
    {

        public StatValueContainer ComboContainer { get; }

        protected float Degen;
        protected float Timeout;
        private float _currentTimeout = 0f;

        public ComboController(BaseUnit owner) : base (owner)
        {

            var cfg = DataManager.Instance.GetConfigByID<ComboStatsConfig>(owner.GetID);
            if (cfg == null)
            {
                IsReady = false;
                return;
            }            
            ComboContainer = new StatValueContainer(cfg.ComboContainer);
            Degen = cfg.DegenCoeff;
            Timeout = cfg.HeatTimeout;
            ComboContainer.Setup();

            IsReady = true;
        }

        #region gameplay

        public bool UseCombo(float value)
        {
            bool result = ComboContainer.GetCurrent >= -value;    // because these are negative in configs
            if (result) ComboContainer.ChangeCurrent(value);
            return result;
        }
        protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            return ComboContainer;
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
            ComboContainer.ChangeCurrent(-Degen * deltaTime);
        }

        #endregion

    }

}