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
            _applied = new Dictionary<StatsEffect, List<BaseEntity>>();
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

        private Dictionary<StatsEffect, List<BaseEntity>> _applied;
        private void HandleTriggerEvent(StatsEffectTriggerEvent obj)
        {
            Debug.Log($"Handling trigger event; {obj}");
            var targetToApply = obj.Target;

                if (_applied.TryGetValue(obj.Applied, out var r))
                {
                    // effect in list

                    if (r.Contains(targetToApply)) return; // target in list
                    else
                    {
                        // target not in list
                        targetToApply.ApplyEffect(obj.Applied);
                        r.Add(obj.Target);

                    if (obj.Applied.OnApply!=null)
                    {
                        obj.Applied.OnApply.GetActionResult().ProduceResult(null, obj.Target, obj.Place); // play particles or maybe something else if needed
                    }
                }
                }
                // effect not in list just do normally
                else
                {
                    targetToApply.ApplyEffect(obj.Applied);
                    _applied[obj.Applied] = new List<BaseEntity>() { targetToApply };
                if (obj.Applied.OnApply != null)
                {
                    obj.Applied.OnApply.GetActionResult().ProduceResult(null, obj.Target, obj.Place);
                }
            }
        }
        }

        #endregion
    }

