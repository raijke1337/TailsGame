using Arcatech.Effects;
using Arcatech.Units;
using System;
using UnityEngine;
namespace Arcatech.Triggers
{
    public abstract class BaseLevelEventTrigger : BaseTrigger
    {
        [SerializeField] protected bool DestroyOnExit = false;
        [SerializeField] protected bool DestroyOnEnter = false;

        public EffectsCollection Effects; // TODO


        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out DummyUnit p))
            {
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
                if (DestroyOnExit)
                {
                    gameObject.SetActive(false);
                }
            }
        }


    }
}