using Arcatech.Actions;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Level
{
    public class InteractiveEventTrigger : BaseLevelEventTrigger, IInteractible
    {
        #region interface
        [Header("Interactive trigger"),Space,SerializeField] protected string _displayName = "Interactive item";
        [SerializeField] protected SerializedActionResult[] ActionOnInteract;

        public string GetName => _displayName;

        public IReadOnlyDictionary<BaseStatType, StatValueContainer> GetDisplayValues => null;

        public Vector3 Position => transform.position;

        public virtual void AcceptInteraction(IInteractible actor)
        {

            foreach (var r in ActionOnInteract)
            {
                r.GetActionResult().ProduceResult(actor as BaseEntity, null, transform);
            }
            actor.AcceptInteraction(this);
        }
        #endregion
    }
}