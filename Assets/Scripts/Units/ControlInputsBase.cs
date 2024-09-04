using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Skills;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units.Inputs;
using Arcatech.Units.Stats;
using KBCore.Refs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            if (DebugMessage)
            {
                Debug.Log($"Do combat action {type}");
            }
            UnitActionRequestedEvent.Invoke(type);
        }

    }
}