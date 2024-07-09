// a predicate is a function that tests a condition and then returns a bool
using Arcatech.Units;
using System;
using UnityEngine;

namespace Arcatech.StateMachine
{
    public abstract class BaseState : IState
    {

        public abstract void FixedUpdate(float d);
        public abstract void HandleCombatAction(UnitActionType action);
        public abstract void OnEnterState();
        public abstract void OnLeaveState();
        public virtual void Update(float d)
        {
            if (TimeLeft > 0) TimeLeft -= d;
        }

        protected const float crossFadeDuration = 0.1f;
        protected readonly ControlledUnit unit;
        protected readonly Animator playerAnimator;
        public float TimeLeft { get; protected set; }

        protected BaseState(ControlledUnit unit, Animator playerAnimator,float timeInState = 0)
        {
            this.unit = unit;
            this.playerAnimator = playerAnimator;
            TimeLeft = timeInState;
        }

        public static readonly int UnarnedStandingIdleHash =
            Animator.StringToHash("UnarmedIdle");        
        public static readonly int UnarmedRotationStandingHash =
            Animator.StringToHash("UnarmedRotationStanding");
        public static readonly int UnarmedLocomotionHash =
            Animator.StringToHash("UnarmedLocomotion");
        public static readonly int Melee2hAttackingHash =
            Animator.StringToHash("Melee2hAttacking");
        public static readonly int AirStateHash =
            Animator.StringToHash("JumpState");
    }



}

