using Arcatech.Actions;
using Arcatech.Effects;
using Arcatech.Units;
using System;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class BaseLevelEventTrigger : BaseTrigger
    {
        [SerializeField] protected bool DestroyOnExit = false;
        [SerializeField] protected bool DestroyOnEnter = false;
        [Space, SerializeField] protected SerializedActionResult[] ActionOnEntry;
        [SerializeField] protected SerializedActionResult[] ActionOnExit;

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out DummyUnit p))
            {
                foreach (var action in ActionOnEntry)
                {
                    action.GetActionResult().ProduceResult(null,p,transform);
                }

                if (DestroyOnEnter)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        protected override void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out DummyUnit p))
            {
                foreach (var action in ActionOnExit)
                {
                    action.GetActionResult().ProduceResult(null, p, transform);
                }

                if (DestroyOnExit)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}