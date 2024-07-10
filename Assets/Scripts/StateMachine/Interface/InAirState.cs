// a predicate is a function that tests a condition and then returns a bool
using Arcatech.Units;
using ECM.Components;
using UnityEngine;

namespace Arcatech.StateMachine
{
    public class InAirState : BaseState
    {
        public InAirState(CharacterMovement movement, ControlledUnit unit, Animator playerAnimator) : base(movement, unit, playerAnimator)
        {
        }

        //private void Pl_OffTheGroundEvent(bool isInAir)
        //{
        //    if (!isInAir) // landed
        //    {
        //        if (pl.AirTime > 1f)
        //        {
                    
        //            playerAnimator.Play("Land");
        //            TimeLeft = playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        //            Debug.Log($"roll");
        //        }
        //        else
        //        {
        //            Debug.Log($"regular landing");
        //        }
        //    }
        //}

        public override void HandleCombatAction(UnitActionType action)
        {

        }
        public override void OnEnterState()
        {
            playerAnimator.CrossFade(AirStateHash, crossFadeDuration);
        }
        public override void OnLeaveState()
        {
            playerAnimator.SetFloat("AirTime", 0);
        }
        public override void Update(float d)
        {
            base.Update(d);
            playerAnimator.SetFloat("AirTime", timer.GetTime);

            if (movement.isOnGround)
            {
                if (timer.GetTime > 2f)
                {
                    playerAnimator.Play("Land");
                }
                timer.Reset();
                timer.Stop();
            }
        }
        public override void FixedUpdate(float d)
        {

        }
    }



}

