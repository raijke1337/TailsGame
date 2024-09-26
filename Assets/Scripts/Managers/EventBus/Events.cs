using Arcatech.Items;
using Arcatech.Skills;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.UI;
using Arcatech.Units;
using CartoonFX;
using UnityEngine;

namespace Arcatech.EventBus
{
    public interface IEvent { }
    public struct PlayerStatsChangedUIEvent : IEvent
    {
        public BaseStatType StatType { get; }
        public StatValueContainer Container { get; }
        public PlayerStatsChangedUIEvent (BaseStatType statType, StatValueContainer container)
        { 
             StatType = statType; Container = container;
        }
    }

    public struct DrawDamageEvent : IEvent
    {
        public BaseEntity Unit { get; }
        public float Damage { get; }
        public DrawDamageEvent (BaseEntity unit, float damage)
        {
            Unit = unit; this.Damage = damage;
        }
    }

    public struct StatsEffectTriggerEvent : IEvent
    {
        public StatsEffectTriggerEvent(BaseEntity target, StatsEffect toApply, Transform place)
        {
            Target = target;
            Applied = toApply;
            Place = place;
        }

        public BaseEntity Target { get; }
        public StatsEffect Applied { get; }
        public Transform Place { get; }
        public override string ToString()
        {
            return string.Concat(Applied," on ", Target?.GetUnitName, " at ", Place.position);
        }
    }
    public struct VFXEvent : IEvent
    {
        public CFXR_Effect Effect;
        public Transform Place;

        public VFXEvent(CFXR_Effect effect, Transform place)
        {
            Effect = effect;
            Place = place;
        }
    }

    public struct PauseToggleEvent : IEvent
    {
        public bool Value { get; }
        public PauseToggleEvent (bool value) => Value = value;
    }

    public struct UpdateIconEvent : IEvent
    {
        public UpdateIconEvent(IIconContent used, BaseEntity user)
        {
            Used = used;
            User = user;
        }
        public IIconContent Used { get; }
        public BaseEntity User { get; }


    }



}