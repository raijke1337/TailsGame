using Arcatech.EventBus;
using Arcatech.Units;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Triggers
{
    public class WeaponTriggerComponent : BaseTrigger
    {

        public event UnityAction<DummyUnit> UnitHitEvent = delegate { };
        public void ToggleCollider(bool isEnable)
        {
            Collider.enabled = isEnable;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<DummyUnit>(out var u))
            {
                TriggerCallback(u, true);
            }
        }
        // weapon signals about trigger hits based on the event
        protected virtual void TriggerCallback(DummyUnit unit, bool entering)
        {
            if (entering)
            {
                UnitHitEvent.Invoke(unit);
            }
        }
    }

}