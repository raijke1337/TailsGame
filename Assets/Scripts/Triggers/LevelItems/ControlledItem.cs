using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Triggers.Items
{
    public abstract class ControlledItem : MonoBehaviour
    {
        [SerializeField] ControlledItemState _startingState;

        protected ControlledItemState _currentState;
        public ControlledItemState GetState => _currentState;
        public void ChangeItemState (ControlledItemState desired)
        {
            if (_currentState != desired)
            {
#if UNITY_EDITOR
                DebugNotify(desired);
#endif
                OnPerformChangeState(desired);
                
            }
        }
        private void DebugNotify(ControlledItemState desired)
        {
            //Debug.Log($"Changing state of item {this} from {_currentState} to {desired}");
        }
        private void Awake()
        {
            // to not override
        }

        private void OnEnable()
        {
            Assert.IsFalse(_startingState == ControlledItemState.None);
            _currentState = ControlledItemState.None;
            InitiateValues();
            ChangeItemState(_startingState);
        }



        protected abstract void OnPerformChangeState(ControlledItemState desired);
        protected abstract void InitiateValues();
    }
}