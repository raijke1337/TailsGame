using Arcatech.Triggers;
using System;
using UnityEngine;

namespace Arcatech.Units
{

    public abstract class ControlInputsBase : MonoBehaviour
    {
        public Vector3 InputsMovementVector { get; protected set; }
        public Vector3 InputsLookVector { get; protected set; }

        protected virtual void OnEnable()
        {
            ControllerBindings(true);
        }
        protected virtual void OnDisable()
        {
            ControllerBindings(false);
        }
        protected abstract ControlInputsBase ControllerBindings(bool start);
        public event Action<UnitActionType> UnitActionRequestedEvent = delegate { };
        public event Action InputsPause = delegate { };
        public event Action<IInteractible> RequestInteraction = delegate { };

        protected virtual void RequestCombatAction(UnitActionType type)
        {
            UnitActionRequestedEvent.Invoke(type);
        }
        
        protected void CallBackPause() => InputsPause.Invoke();
        protected void CallBackInteraction(IInteractible i) => RequestInteraction.Invoke(i);
    }
}