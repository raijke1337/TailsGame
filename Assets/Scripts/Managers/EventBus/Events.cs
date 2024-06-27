using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units;

namespace Arcatech.EventBus
{
    public interface IEvent { }
    public struct StatChangedEvent : IEvent
    {
        public BaseStatType StatType { get; }
        public StatValueContainer Container { get; }
        public StatChangedEvent (BaseStatType statType, StatValueContainer container)
        { 
        StatType = statType; Container = container;
        }
    }

    public struct DrawDamageEvent : IEvent
    {
        public BaseUnit Unit { get; }
        public float Damage { get; }
        public DrawDamageEvent (BaseUnit unit, float damage)
        {
            Unit = unit; this.Damage = damage;
        }
    }

    public struct StatsEffectTriggerEvent : IEvent
    {
        public StatsEffectTriggerEvent(DummyUnit target, DummyUnit source, bool isEnteringTrigger, StatsEffect[] appliedEffects)
        {
            Target = target;
            Source = source;
            IsEnteringTrigger = isEnteringTrigger;
            AppliedEffects = appliedEffects;
        }

        public DummyUnit Target { get; }
        public DummyUnit Source { get; }
        public bool IsEnteringTrigger { get; }
        public StatsEffect[] AppliedEffects { get; }


    }



}