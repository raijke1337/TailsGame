using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Triggers
{
    public abstract class ConditionControlledItem : MonoBehaviour
    {
        [SerializeField] ControlledItemState _startingState;
        public event SimpleEventsHandler<ControlledItemState, ConditionControlledItem> ItemChangedStateEvent;



        protected ControlledItemState _currentState;
        public void ChangeItemState (ControlledItemState desired)
        {
            if (_currentState != desired)
            {
                OnPerformChangeState(desired);                
            }
        }

        private void Awake()
        {
            // to not override
        }

        private void OnEnable()
        {
            Assert.IsFalse(_startingState == ControlledItemState.None);
            _currentState = ControlledItemState.None;
            StartControllerValues();
            ChangeItemState(_startingState);
        }


        protected void CallbackEvent(ControlledItemState state)=> ItemChangedStateEvent?.Invoke(state, this);
        protected abstract void OnPerformChangeState(ControlledItemState desired);
        protected abstract void StartControllerValues();
    }
}