using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public abstract class BaseController : IStatsComponentForHandler, IHasOwner, IHasEffects
    {
        protected List<TriggeredEffect> _activeEffects = new List<TriggeredEffect>();
        public BaseUnit Owner { get; }

        private bool _isReady = false;

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
            foreach (var ef in _activeEffects)
            {
                // actual handling
                StatValueContainer stat = SelectStatValueContainer(ef);
                // start effect
                if (!ef.InitialDone)
                {
                    stat.ChangeCurrent(ef.InitialValue);
                    ef.InitialDone = true;
                }
                // remove expired or instantaneous
                if (ef.TotalDuration <= 0f)
                {
                    _activeEffects.Remove(ef);
                    return;
                }
                // handle timers    
                ef.CurrentRepeatTimer -= deltaTime;
                ef.TotalDuration -= deltaTime;
                if (ef.CurrentRepeatTimer <= 0f)
                {
                    stat.ChangeCurrent(ef.RepeatedValue);
                    ef.CurrentRepeatTimer = ef.RepeatApplicationDelay;
                }
            }
        }

        public virtual void PickTriggeredEffectHandler(TriggeredEffect effect)
        {
            _activeEffects.Add(effect);
        }

        protected virtual StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
        {
            Debug.LogWarning($"{this} was requested to select a StatValueContainer by effect {effect}, and nothing happened");
            return null;
        }
        #region debug


        protected float currentTimer = 0;
        [SerializeField] protected float debugTime;
        [SerializeField] protected bool debugEnabled;

        #endregion


        #region managed
        public virtual void UpdateInDelta(float deltaTime)
        {
            if (debugEnabled)
            {
                currentTimer += deltaTime;
                if (currentTimer > debugTime)
                {
                    Debug.Log($"{this} on {Owner.name} ready: {IsReady}");
                    currentTimer = 0;
                }
            }

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

        #region triggers from weapons and /skills (todo)/

        public event TriggerEvent BaseControllerTriggerEvent;
        protected void TriggerEventCallback(BaseUnit target, BaseUnit source, bool isEnter, BaseStatTriggerConfig cfg) => BaseControllerTriggerEvent?.Invoke(target, source, isEnter, cfg);

        #endregion

        #region projectile spawn

        public event SimpleEventsHandler<ProjectileComponent> SpawnProjectileEvent;

        protected void SpawnProjectileCallBack(ProjectileComponent proj)
        {
            SpawnProjectileEvent?.Invoke(proj);
        }

        #endregion


    }

}