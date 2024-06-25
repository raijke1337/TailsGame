using Arcatech.Stats;
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
}