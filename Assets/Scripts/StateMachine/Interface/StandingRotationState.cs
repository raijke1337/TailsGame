// a predicate is a function that tests a condition and then returns a bool
using Arcatech.Units;
using ECM.Components;
using UnityEngine;

namespace Arcatech.StateMachine
{
    public class StandingRotationState : BaseState
    {
        public StandingRotationState(CharacterMovement movement, ControlledUnit unit, Animator playerAnimator) : base(movement, unit, playerAnimator)
        {
        }

        public override void FixedUpdate(float d)
        {
        }

        public override void HandleCombatAction(UnitActionType action)
        {

        }
        public override void OnEnterState()
        {
            playerAnimator.CrossFade(UnarmedRotationStandingHash, crossFadeDuration);
            //TimeLeft = playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }

        public override void OnLeaveState()
        {

        }
        public override void Update(float d)
        {
            unit.DoRotationInDeltaTime();
        }
    }

}

