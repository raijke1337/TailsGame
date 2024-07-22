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
        public StatsEffectTriggerEvent(BaseUnit target, BaseUnit source, bool isEnteringTrigger, Transform place, StatsEffect[] appliedEffects)
        {
            Target = target;
            Source = source;
            IsEnteringTrigger = isEnteringTrigger;
            AppliedEffects = appliedEffects;
            Place = place;
        }

        public BaseUnit Target { get; }
        public BaseUnit Source { get; }
        public bool IsEnteringTrigger { get; }
        public StatsEffect[] AppliedEffects { get; }
        public Transform Place { get; }
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

    public struct PlayerPauseEvent : IEvent
    {
        public bool Value { get; }
        public PlayerPauseEvent(bool v) => Value = v;
    }

    public struct UpdateIconEvent : IEvent
    {
        public UpdateIconEvent(IIconContent used, BaseUnit user)
        {
            Used = used;
            User = user;
        }
        public IIconContent Used { get; }
        public BaseUnit User { get; }


    }



}