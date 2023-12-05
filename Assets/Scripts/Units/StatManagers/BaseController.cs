using Arcatech.Triggers;
using Arcatech.Units;
using CartoonFX;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public abstract class BaseController : IStatsComponentForHandler, IProducesSounds, IHasOwner
    {
        protected List<TriggeredEffect> _activeEffects = new List<TriggeredEffect>();
        public BaseUnit Owner { get; }

        private bool _isReady = false;

        public event SimpleEventsHandler<bool, IStatsComponentForHandler> ComponentChangedStateToEvent;

        public BaseController (BaseUnit owner)
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

        public virtual void AddTriggeredEffect(TriggeredEffect effect)
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

        #region effects

        public event AudioEvents UnitRequestsSound;
        public event SimpleEventsHandler<CFXR_Effect,Transform> EffectsParticlePlace;
        protected virtual void SoundPlayCallback(AudioClip c)
        {
            if (c == null) return;
           UnitRequestsSound?.Invoke(c, Vector3.zero);
        }
        protected virtual void ParticlesPlayCallback(CFXR_Effect eff,Transform where)
        {
            if (eff == null) return;
            EffectsParticlePlace?.Invoke(eff,where);
        }


        #endregion

    }

}