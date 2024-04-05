using Arcatech.Units;
using UnityEditorInternal;
using UnityEngine;
namespace Arcatech.Triggers
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseTrigger : MonoBehaviour
    {


        #region trigger
        [SerializeField] protected bool forceTrigger = true;

        public event SimpleTriggerEvent TriggerHitUnitEvent;
        protected void TriggerCallback(BaseUnit tgt, bool entering)
        {
            TriggerHitUnitEvent?.Invoke(tgt, entering);
        }


        #endregion

        public Collider Collider { get; protected set; }
        protected virtual void Start()
        {
            Collider = GetComponent<Collider>();
            if (forceTrigger)
            {
                Collider.isTrigger = true;
            }
        }

        protected abstract void OnTriggerEnter(Collider other);
        protected abstract void OnTriggerExit(Collider other);

    }

}