using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Triggers
{
    public abstract class BaseLevelEventTrigger : BaseTrigger
    {
        protected override void Start()
        {
            base.Start();
            Collider.isTrigger = true; 
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerUnit p))
            {
                TriggerCallback(p, true);
                OnEnter();
            }
        }
        protected override void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerUnit p))
            {
                TriggerCallback(p, true);
                OnExit();
            }
        }
        protected abstract void OnEnter();
        protected abstract void OnExit();


    }
}