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
    public abstract class BaseController : IManagedComponent, IHasOwner
    {
        public BaseUnit Owner { get; }

        private bool _isReady = false;

        public event SimpleEventsHandler<bool, IManagedComponent> ComponentChangedStateToEvent;

        public BaseController(BaseUnit owner)
        {
            Owner = owner;
        }


        protected void StateChangeCallback(bool ready, IManagedComponent comp)
        {
            ComponentChangedStateToEvent?.Invoke(ready, comp);
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

        public abstract void ApplyEffect(TriggeredEffect effect);

        #region managed
        public abstract void UpdateInDelta(float deltaTime);
        public abstract void StartComp();
        public abstract void StopComp();
        #endregion


        #region Effects

        // todo move this stuff to an events bus system as watched

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