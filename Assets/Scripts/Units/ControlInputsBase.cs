using System;
using UnityEngine;

namespace Arcatech.Units
{

    public abstract class ControlInputsBase : MonoBehaviour
    {
        [SerializeField] public bool DebugMessage = false;
        public Vector3 InputsMovementVector { get; protected set; }
        public Vector3 InputsLookVector { get; protected set; }
        //public ControlledUnit Unit { get; set; }

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


        protected virtual void RequestCombatAction(UnitActionType type)
        {
            UnitActionRequestedEvent.Invoke(type);
        }

    }
}