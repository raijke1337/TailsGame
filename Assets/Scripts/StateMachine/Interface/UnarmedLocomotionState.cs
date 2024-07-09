// a predicate is a function that tests a condition and then returns a bool
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.StateMachine
{
    public class UnarmedLocomotionState : BaseState
    {
        public UnarmedLocomotionState(ControlledUnit unit, Animator playerAnimator) : base(unit, playerAnimator)
        {
        }

        public override void FixedUpdate(float d)
        {
            unit.DoHorizontalMovement(d);
            unit.DoRotation(d);
        }

        public override void HandleCombatAction(UnitActionType action)
        {
            
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            playerAnimator.CrossFade(UnarmedLocomotionHash, crossFadeDuration);
        }

        public override void OnLeaveState()
        {

        }

        public override void Update(float d)
        {
            base.Update(d);
        }
    }

}

