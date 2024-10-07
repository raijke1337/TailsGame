using Arcatech.Triggers;
using AYellowpaper.SerializedCollections;
using UnityEngine;
namespace Arcatech.Level
{
    public class InteractiveConditionChecker : InteractiveEventTrigger
    {
        [Space, Header("Condition checker")]
        [SerializeField] SerializedDictionary<EventCondition, ConditionControlledItem[]> _list;

        public override void AcceptInteraction(IInteractor actor)
        {
            base.AcceptInteraction(actor);
            foreach (var cond in _list.Keys)
            {
                bool rse = cond.PerformConditionChecks(actor, this, transform);
                foreach (ConditionControlledItem item in _list[cond])
                {
                    item.SetState(rse);
                }
            }
        }
    }
}