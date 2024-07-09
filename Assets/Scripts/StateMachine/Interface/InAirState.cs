// a predicate is a function that tests a condition and then returns a bool
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.StateMachine
{
    public class InAirState : BaseState
    {
        GroundDetectorPlatformCollider pl;
        public InAirState(GroundDetectorPlatformCollider platform, ControlledUnit unit, Animator playerAnimator, float maxTimeInState = 0) : base(unit, playerAnimator, maxTimeInState)
        {
            pl = platform;
            pl.OffTheGroundEvent += Pl_OffTheGroundEvent;            
        }

        private void Pl_OffTheGroundEvent(bool isInAir)
        {
            if (!isInAir) // landed
            {
                if (pl.AirTime > 1f)
                {
                    playerAnimator.SetFloat("AirTime", 0f);
                    playerAnimator.Play("Land");
                    TimeLeft = playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                    Debug.Log($"roll");
                }
                else
                {
                    Debug.Log($"regular landing");
                }
            }
        }

        public override void FixedUpdate(float d)
        {
            playerAnimator.SetFloat("AirTime",pl.AirTime);            
        }

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
    }



}

