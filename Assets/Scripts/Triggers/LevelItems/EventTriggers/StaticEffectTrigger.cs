using Arcatech.EventBus;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Triggers
{
    public class StaticEffectTrigger : WeaponTriggerComponent
    {
        [SerializeField] SerializedStatsEffectConfig[] Triggers;
        private StatsEffect[] _currentEffects;

        protected override void Awake()
        {
            base.Awake();
            _currentEffects = new StatsEffect[Triggers.Length]; 
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent <PlayerUnit>(out var p))
            {
                TriggerCallback(p, true);
            }
        }
        protected override void TriggerCallback(DummyUnit unit, bool entering)
        {
            if (entering)
            {
                for (int i = 0; i < Triggers.Length; i++)
                {
                    _currentEffects[i] = new StatsEffect(Triggers[i]);
                }
                EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(unit,null,entering,unit.transform, _currentEffects));
            }
        }// send signal itself
    }
}