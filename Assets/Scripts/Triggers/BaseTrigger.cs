using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Triggers
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseTrigger : MonoBehaviour, IAppliesTriggers
    {

        protected Collider _coll;
        public event TriggerEventApplication TriggerApplicationRequestEvent;

        protected void TriggerCallback(string ID, BaseUnit unit, BaseUnit source) => TriggerApplicationRequestEvent?.Invoke(ID, unit, source);
        // triggers must be registered with triggers manager in child classes

        protected virtual void Start()
        {
            _coll = GetComponent<Collider>();
            _coll.isTrigger = true;
        }

        protected abstract void OnTriggerEnter(Collider other);

        public GameObject GetObject() => gameObject;

    }

}