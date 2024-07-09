// a predicate is a function that tests a condition and then returns a bool
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.StateMachine
{
    public class UnarmedIdleState : BaseState
    {
        public UnarmedIdleState(ControlledUnit inputs, Animator playerAnimator, float maxTimeInState = 0) : base(inputs, playerAnimator, maxTimeInState)
        {
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            playerAnimator.CrossFade(UnarnedStandingIdleHash, crossFadeDuration);
        }
        public override void FixedUpdate(float d)
        {
            
        }
        public override void Update(float d)
        {
            base.Update(d);
            unit.DoRotation(d);
        }

        public override void HandleCombatAction(UnitActionType action)
        {

        }

        public override void OnLeaveState()
        {

        }
    }

}

