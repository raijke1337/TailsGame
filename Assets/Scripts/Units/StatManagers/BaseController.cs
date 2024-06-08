using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public abstract class BaseController : IStatsComponentForHandler, IHasOwner, IHasEffects
    {
        protected List<TriggeredEffect> _activeEffects = new List<TriggeredEffect>();
        public BaseUnit Owner { get; }

        private bool _isReady = false;
        public abstract string GetUIText { get; }

        public event SimpleEventsHandler<bool, IStatsComponentForHandler> ComponentChangedStateToEvent;

        public BaseController(BaseUnit owner)
        {
            Owner = owner;
        }


        protected void StateChangeCallback(bool ready, IStatsComponentForHandler comp)
        {
            ComponentChangedStateToEvent?.Invoke(ready, comp);
            //Debug.Log($"{this} called state change with value {val}");
        }
        public virtual bool IsReady
        {
            get => _isReady;
            protected set
            {
                _isReady = value;
                StateChangeCallback(value, this);
            }
        }

        public virtual void StopStatsComponent()
        {
            //  Debug.Log($"{this} was stopped");
        }

        public virtual void HandleEffects(float deltaTime)
        {
            if (!IsReady || _activeEffects.Count == 0) return;
            foreach (TriggeredEffect ef in _activeEffects)
            {
                // actual handling\
                StatValueContainer stat = SelectStatValueContainer(ef);
                // start effect
                if (!ef.InitialDone)
                {
                    stat.ChangeCurrent(ef.InitialValue);
                    ef.InitialDone = true;
                }
                // remove expired or instantaneous

                // handle timers    
                ef.CurrentRepeatTimer -= deltaTime;
                ef.TotalDuration -= deltaTime;
                if (ef.CurrentRepeatTimer <= 0f)
                {
                    stat.ChangeCurrent(ef.RepeatedValue);
                    ef.CurrentRepeatTimer = ef.RepeatApplicationDelay;
                }
            }
            foreach (var ef in _activeEffects.ToList())
            {
                if (ef.TotalDuration <= 0f)
                {
                    _activeEffects.Remove(ef);
                    return;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        /// <returns>if result is negative</returns>
        public virtual void ApplyEffectToController(TriggeredEffect effect)
        {

            _activeEffects.Add(effect);

        }

        protected virtual StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            Debug.LogWarning($"{this} was requested to select a StatValueContainer by effect {effect}, and nothing happened");
            return null;
        }


        #region managed
        public virtual void UpdateInDelta(float deltaTime)
        {


            if (!IsReady) return;
            HandleEffects(deltaTime);
        }
        public virtual void SetupStatsComponent()
        {

        }
        #endregion


        #region Effects

        public event EffectsManagerEvent BaseControllerEffectEvent;
        protected void EffectEventCallback(EffectRequestPackage pack) => BaseControllerEffectEvent?.Invoke(pack);

        #endregion

        #region triggers from weapons and /skills 

        public event TriggerEvent BaseControllerTriggerEvent;
        protected void TriggerEventCallback(BaseUnit target, BaseUnit source, bool isEnter, TriggeredEffect cfg)
        {
            //Debug.Log($"Weapon trigger event: hit {target.name} from {source.name}");
            BaseControllerTriggerEvent?.Invoke(target, source, isEnter, cfg);
        }
        #endregion

        #region projectile spawn

        public event SimpleEventsHandler<ProjectileComponent> BaseControllerProjectileEvent;

        protected void SpawnProjectileCallBack(ProjectileComponent proj)
        {
            BaseControllerProjectileEvent?.Invoke(proj);
        }

        #endregion


    }

}