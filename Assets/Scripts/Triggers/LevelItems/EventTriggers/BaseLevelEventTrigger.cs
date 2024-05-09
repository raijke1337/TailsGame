using Arcatech.Units;
using UnityEngine;
namespace Arcatech.Triggers
{
    public abstract class BaseLevelEventTrigger : BaseTrigger
    {
        [SerializeField] protected bool DestroyOnExit = false;
        [SerializeField] protected bool DestroyOnEnter = false;
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
                if (DestroyOnEnter)
                {
                    gameObject.SetActive(false);
                }
            }

        }
        protected override void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerUnit p))
            {
                TriggerCallback(p, false);
                OnExit();
            }
            if (DestroyOnExit)
            {
                gameObject.SetActive(false);
            }
        }
        protected abstract void OnEnter();
        protected abstract void OnExit();


    }
}