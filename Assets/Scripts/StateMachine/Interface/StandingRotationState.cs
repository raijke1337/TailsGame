// a predicate is a function that tests a condition and then returns a bool
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.StateMachine
{
    public class StandingRotationState : BaseState
    {
        public StandingRotationState(ControlledUnit inputs, Animator playerAnimator, float maxTimeInState = 0) : base(inputs, playerAnimator, maxTimeInState)
        {
            
        }

        public override void FixedUpdate(float d)
        {
            unit.DoRotation(d);
            
        }

        public override void HandleCombatAction(UnitActionType action)
        {

        }
        public override void OnEnterState()
        {
            playerAnimator.CrossFade(UnarmedRotationStandingHash, crossFadeDuration);
            TimeLeft = playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }

        public override void OnLeaveState()
        {

        }
    }

}

