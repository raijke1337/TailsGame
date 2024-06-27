using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

namespace Arcatech.Triggers.Items
{


    public class ControlledItemMoves : ConditionControlledItem
    {

        [SerializeField] protected Vector3 _desiredChangeLocalPosition;
        [SerializeField] protected Vector3 _desiredChangeLocalEulers;
        [SerializeField] protected float _movementTime;

        private float lerpKoef;

        private Vector3 _negPos;
        private Vector3 _negEulers;
        private Vector3 _posPos;
        private Vector3 _posEulers;

        private bool isBusy = false;


        protected override void StartControllerValues()
        {
            _negPos = transform.localPosition;
            _negEulers = transform.localEulerAngles;
            _posPos = _negPos + _desiredChangeLocalPosition;
            _posEulers = _negEulers + _desiredChangeLocalEulers;


            lerpKoef = 1 / _movementTime;
        }

        protected override void OnPerformChangeState(ControlledItemState desired)
        {
            if (isBusy) return;
            switch (desired)
            {
                // todo maybe make this a state machine instead
                case ControlledItemState.Negative:
                    isBusy = true;
                    StartCoroutine(Move(false));
                    break;

                case ControlledItemState.Positive:

                    isBusy = true;
                    StartCoroutine(Move(true)); break;
                default:
                    break;

            }
        }

        protected IEnumerator Move(bool isPositive)
        {
            float progress = 0;

            if (isPositive)
            {
                while (progress < 1)
                {
                    _currentState = ControlledItemState.NegativeToPositive;
                    CallbackEvent(ControlledItemState.NegativeToPositive);
                    progress += 1 / _movementTime * Time.deltaTime;
                    transform.localPosition = Vector3.Lerp(_negPos, _posPos, progress);
                    transform.localEulerAngles = Vector3.Lerp(_negEulers, _posEulers,progress);
                    yield return null;
                }
                isBusy = false;
                _currentState = ControlledItemState.Positive;
                CallbackEvent(ControlledItemState.Positive);
                yield return null;
            }
            else
            {
                while (progress < 1)
                {
                    _currentState = ControlledItemState.PositiveToNegative;
                    CallbackEvent(ControlledItemState.PositiveToNegative);
                    progress += 1 / _movementTime * Time.deltaTime;
                    transform.localPosition = Vector3.Lerp(_posPos, _negPos, progress);
                    transform.localEulerAngles = Vector3.Lerp(_posEulers, _negEulers, progress);
                    yield return null;
                }
                isBusy = false;
                CallbackEvent(ControlledItemState.Negative);
                _currentState = ControlledItemState.Negative;
                yield return null;
            }

        }
    }
}