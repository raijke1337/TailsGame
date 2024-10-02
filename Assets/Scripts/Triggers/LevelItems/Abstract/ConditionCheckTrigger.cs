using Arcatech.Level;
using Arcatech.Units;
using AYellowpaper.SerializedCollections;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class ConditionCheckTrigger : BaseLevelEventTrigger
    {
        [Space, Header("Condition check trigger"),SerializeField] SerializedDictionary <SerializedActionCondition, ConditionControlledItem[]> CheckOnEnter;
        [SerializeField] SerializedDictionary <SerializedActionCondition, ConditionControlledItem[]> CheckOnExit;
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.gameObject.TryGetComponent(out PlayerUnit p))
            {
                foreach (var check in CheckOnEnter.Keys)
                {
                    bool ok = check.PerformConditionChecks(p, null, null);
                    foreach (var item in CheckOnEnter[check])
                    {
                        item.SetState(ok);
                    }
                }
            }
        }
        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            if (other.gameObject.TryGetComponent(out PlayerUnit p))
            {
                foreach (var check in CheckOnExit.Keys)
                {
                    bool ok = check.PerformConditionChecks(p, null, null);
                    foreach (var item in CheckOnExit[check])
                    {
                        item.SetState(ok);
                    }
                }
            }
        }

    }
}