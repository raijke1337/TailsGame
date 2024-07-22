using Arcatech.Effects;
using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.Managers
{
    public class TriggersManager : MonoBehaviour, IManagedController
    {

        EventBinding<StatsEffectTriggerEvent> _triggersBinding;

        #region LoadedManager
        public virtual void StartController()
        {
            _triggersBinding = new EventBinding<StatsEffectTriggerEvent>(HandleTriggerEvent);
            _applied = new Dictionary<StatsEffect, List<BaseUnit>>();
            EventBus<StatsEffectTriggerEvent>.Register(_triggersBinding);
        }
        public virtual void ControllerUpdate(float delta)
        {

        }
        public virtual void FixedControllerUpdate(float fixedDelta)
        {

        }

        public virtual void StopController()
        {
            _applied.Clear();
            EventBus<StatsEffectTriggerEvent>.Deregister(_triggersBinding);
        }
        #endregion

        #region triggers

        private Dictionary<StatsEffect, List<BaseUnit>> _applied;
        private void HandleTriggerEvent(StatsEffectTriggerEvent obj)
        {
            if (obj.AppliedEffects == null) return; // happens sometimes because collidertoggle was not properly run on a weapon - TODO
            foreach (StatsEffect eff in obj.AppliedEffects)
            {

                // determine target
                DummyUnit targetToApply = DetermineTarget(obj,eff) as DummyUnit;

                if (targetToApply == null) return;

                // check if target already had the effect instance applied


                if (_applied.TryGetValue(eff, out var r))
                {
                    // effect in list

                    if (r.Contains(targetToApply)) return; // target in list
                    else
                    {
                        // target not in list
                        targetToApply.ApplyEffect(eff);
                        r.Add(obj.Target);
                    }
                }
                // effect not in list just do normally
                else
                {
                    targetToApply.ApplyEffect(eff);
                    _applied[eff] = new List<BaseUnit>() { targetToApply };
                }

                if (eff.GetEffects.TryGetEffect(EffectMoment.OnCollision, out var e))
                {
                    EventBus<VFXRequest>.Raise(new VFXRequest(e, obj.Place));
                }

            }
        }
        BaseUnit DetermineTarget (StatsEffectTriggerEvent ev, StatsEffect eff)
        {
            switch (eff.Target)
            {
                case TriggerTargetType.TargetsEnemies:
                    if (ev.Source == null || ev.Source.Side != ev.Target.Side)
                    {
                        return ev.Target;
                    }
                    break;
                case TriggerTargetType.TargetsUser:
                    return ev.Source;

                case TriggerTargetType.TargetsAllies:
                    if (ev.Source.Side == ev.Target.Side && ev.Source != ev.Target)
                    {
                        return ev.Target;
                    }
                    break;
            }
            return null;
        }

        #endregion
    }

}