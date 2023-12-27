using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class WeaponTriggerComponent : BaseTrigger
    {
        public void ToggleCollider(bool isEnable)
        {
            if (Collider == null) Collider = GetComponent<Collider>();
            Collider.enabled = isEnable;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BaseUnit>(out var c))
            {
                //Debug.Log($"Trigger callback by {this.gameObject.name} on {c}");
                TriggerCallback(c, true);
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<BaseUnit>(out var c))
            {
                TriggerCallback(c, false);
            }
        }
    }

}