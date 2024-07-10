// a predicate is a function that tests a condition and then returns a bool
using Arcatech.Units;
using ECM.Components;
using UnityEngine;

namespace Arcatech.StateMachine
{
    public class UnarmedLocomotionState : BaseState
    {
        public UnarmedLocomotionState(CharacterMovement movement, ControlledUnit unit, Animator playerAnimator) : base(movement, unit, playerAnimator)
        {
        }

        public override void FixedUpdate(float d)
        {
            unit.DoMovementInFixedTime();

        }
        public override void Update(float d)
        {base.Update(d);
            unit.DoRotationInDeltaTime();
        }

        public override void HandleCombatAction(UnitActionType action)
        {
            
        }

        public override void OnEnterState()
        {

            playerAnimator.CrossFade(UnarmedLocomotionHash, crossFadeDuration);
        }

        public override void OnLeaveState()
        {

        }
    }

}

