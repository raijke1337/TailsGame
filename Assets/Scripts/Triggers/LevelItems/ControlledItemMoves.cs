using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

namespace Arcatech.Triggers.Items
{
    public enum ControlledItemState : byte
    {
        None,
        Closed,
        Opening,
        Open,
        Closing   
    }

    public class ControlledItemMoves : ControlledItem
    {

        [SerializeField] protected Vector3 _desiredChangeLocalPosition;
        [SerializeField] protected Vector3 _desiredChangeLocalEulers;
        [SerializeField] protected float _movementTime;

        private Transform _closedTr;
        private Transform _openTr;
        private ControlledItemState _state;
        private ControlledItemState _desiredState;

        private void OnEnable()
        {
            _closedTr = transform;
            _openTr = _closedTr;
            _openTr.localPosition += _desiredChangeLocalPosition;
            _openTr.localEulerAngles += _desiredChangeLocalEulers;
            _state = ControlledItemState.Closed;
            _desiredState = ControlledItemState.Closed;

        }
        public override void DoControlAction(bool isP)
        {
            if (isP) _desiredState = ControlledItemState.Open;
            else _desiredState = ControlledItemState.Closed;
        }

        private void Update()
        {
            if (_state != _desiredState)
            {
                switch (_desiredState)
                {
                    case ControlledItemState.Closed:
                        _state = ControlledItemState.Closing;
                        if (PerformMovement(_closedTr)) _state = ControlledItemState.Closed;
                        break;
                    case ControlledItemState.Open:
                        _state = ControlledItemState.Opening;
                        if (PerformMovement(_openTr)) _state = ControlledItemState.Open;
                        break;
                        default:
                        Debug.Log(this.name + " has a broken action script");
                        break;
                }
            }
        }
        protected bool PerformMovement(Transform finalState)
        {
            if (finalState == transform)
            {
                return true;
            }
            else
            {
                transform.localPosition += Vector3.Lerp(transform.localPosition, finalState.localPosition, Time.deltaTime);
                transform.localEulerAngles += Vector3.Lerp(transform.localEulerAngles, finalState.eulerAngles, Time.deltaTime);
                return false;
            }
        }


    }
}