using Arcatech.Managers;
using UnityEngine;
namespace Arcatech.Triggers
{
    public class LevelEventTrigger : BaseTrigger
    {
        public SimpleEventsHandler<LevelEventTrigger, bool> EnterEvent;
        public LevelEventType EventType;
        [Tooltip("Usage depends on type. For Text - containerID. For Pickup - itemID")] public string ContentIDString;

        protected override void Start()
        {
            base.Start();
            _coll.isTrigger = true; // wtf
            GameManager.Instance.GetGameControllers.EventTriggersManager.RegisterEventTrigger(this);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            EnterEvent?.Invoke(this, true);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            EnterEvent?.Invoke(this, false);
        }
    }
}