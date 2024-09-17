using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
using Arcatech.EventBus;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New apply stat change result", menuName = "Actions/Action Result/Apply Stat Change", order = 3)]
    public class SerializedApplyStatChangeResult : SerializedActionResult
    {
        [SerializeField] SerializedDictionary<TriggerTargetType, SerializedStatsEffectConfig[]> StatChanges;
        public override IActionResult GetActionResult()
        {
            return new ApplyStatChangeEffectResult(StatChanges);
        }
    }
    public class ApplyStatChangeEffectResult : ActionResult
    {
        Dictionary <TriggerTargetType,StatsEffect[]> _effs; 
        public ApplyStatChangeEffectResult(SerializedDictionary<TriggerTargetType, SerializedStatsEffectConfig[]> cfg)
        {
            _effs = new();
            foreach (var type in cfg.Keys)
            {
                StatsEffect[] effects = new StatsEffect[cfg[type].Length];
                for (int i = 0; i < cfg[type].Length; i++)
                {
                    effects[i] = new StatsEffect(cfg[type][i]); 
                }
                _effs.Add(type, effects);
            }
        }

        public override void ProduceResult(BaseEntity user, BaseEntity target,Transform place)
        {
            foreach (var type in _effs.Keys)
            {
                switch (type)
                {
                    case TriggerTargetType.None:
                        foreach (var e in _effs[type])
                        {
                            Debug.LogWarning($"Target type not set for effect {e}");
                        }
                        break;
                    case TriggerTargetType.OnlyUser:
                        foreach (var e in _effs[type])
                        {
                            EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(user as DummyUnit, e, place));
                        }
                        break;
                    case TriggerTargetType.AnyUnit:
                        foreach (var e in _effs[type])
                        {
                            EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(target as DummyUnit, e, place));
                        }
                        break;
                    case TriggerTargetType.AnyEnemy:
                        if (target.Side == user.Side) return;
                        foreach (var e in _effs[type])
                        {
                            EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(target as DummyUnit, e, place));
                        }
                        break;
                    case TriggerTargetType.AnyAlly:
                        if (target.Side != user.Side) return;
                        foreach (var e in _effs[type])
                        {
                            EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(target as DummyUnit, e, place));
                        }
                        break;
                }
            }
        }
    }

}