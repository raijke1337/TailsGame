using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Triggers.Items
{
    public class ControlledItemLightChangesColor : ConditionControlledItem
    {
        [SerializeField] private Light _light;
        [SerializeField] private Color _negColor;
        [SerializeField] private Color _posColor;

        protected override void OnPerformChangeState(ControlledItemState desired)
        {
            switch (desired)
            { 
                case ControlledItemState.Negative:
                    CallbackEvent(ControlledItemState.Negative);
                    _light.color = _negColor; 
                    break;
                case ControlledItemState.Positive:
                    CallbackEvent(ControlledItemState.Positive);
                    _light.color = _posColor;
                    break;
                default:
                    break;

            }
        }

        protected override void StartControllerValues()
        {
            Assert.IsNotNull(_light,$"Pick light object in {this}");
        }
    }
}