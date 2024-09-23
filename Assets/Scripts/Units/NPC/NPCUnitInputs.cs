using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units.Inputs
{
    public class NPCUnitInputs : ControlInputsBase
    {
        protected void SetLookVector(Vector3 v) => InputsLookVector = v;
        protected void SetMoveVector(Vector3 v) => InputsMovementVector = v;
        protected override ControlInputsBase ControllerBindings(bool start)
        {
            return this;
        }
        protected override void RequestCombatAction(UnitActionType type)
        {
            base.RequestCombatAction(type);
        }




    }
}