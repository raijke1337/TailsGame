using Arcatech.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AI System/Decision/Assess Stat")]
    public class AssessBaseStatDecision : Decision
    {
        [Range(0, 1f)] public float PositivePercentage;
        public ReferenceUnitType CheckedUnit;
        public TriggerChangedValue CheckedStat;

        public override bool Decide(StateMachine controller)
        {
            BaseUnit ch;
            switch (CheckedUnit)
            {
                case ReferenceUnitType.Self:
                    ch = controller.ControlledUnit;
                    break;
                case ReferenceUnitType.Player:
                    ch = controller.Player;
                    break;
                case ReferenceUnitType.Focus:
                    ch = controller.FocusUnit;
                    break;
                default:
                    Debug.Log($"{controller.ControlledUnit.GetFullName} tried to assess unit type {CheckedUnit}, which is NYI");
                    return false;
            }
            var cont = ch.GetInputs().AssessStat(CheckedStat);

            if (cont == null)
            {
                Debug.Log($"{controller.ControlledUnit.GetFullName} tried to assess unit type {CheckedUnit}, for stat {CheckedStat} but the container is not available");
                return false;
            }
            else
            {
                var perc = cont.GetCurrent / cont.GetMax;
                return perc < PositivePercentage;
            }
        }
    }
}