using Arcatech.Managers;
using UnityEngine;
namespace Arcatech.Triggers
{
    public abstract class BaseLevelEventTrigger : BaseTrigger
    {
        public SimpleEventsHandler<BaseLevelEventTrigger, bool> PlayerTagTriggerEvent;

        protected override void Start()
        {
            base.Start();
            Collider.isTrigger = true; // wtf
            //GameManager.Instance.GetGameControllers.EventTriggersManager.RegisterEventTrigger(this);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            PlayerTagTriggerEvent?.Invoke(this, true);
        }
        protected override void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            PlayerTagTriggerEvent?.Invoke(this, false);
        }


    }
}