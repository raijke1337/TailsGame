// a predicate is a function that tests a condition and then returns a bool
using Arcatech.Units;
using System;
using UnityEngine;

namespace Arcatech.StateMachine
{
    public abstract class BaseState : IState
    {
        public event Action StateTimeOutEvent = delegate { };
        protected void StateTimeoutCallback() => StateTimeOutEvent?.Invoke();

        public abstract void FixedUpdate(float d);
        public abstract void HandleCombatAction(UnitActionType action);
        public virtual void OnEnterState()
        {
            Debug.Log($"{unit.name} enter state {this}");
        }
        public abstract void OnLeaveState();
        public virtual void Update(float d)
        {
            if (stateExitTimer == null) return;
            stateExitTimer.Tick(d);
        }

        protected const float crossFadeDuration = 0.1f;
        protected readonly ControlledUnit unit;
        protected readonly Animator playerAnimator;
        protected CountDownTimer stateExitTimer;

        protected BaseState(ControlledUnit inputs, Animator playerAnimator,float maxTimeInState = 0)
        {
            this.unit = inputs;
            this.playerAnimator = playerAnimator;
            if (maxTimeInState > 0)
            {
                stateExitTimer = new CountDownTimer(maxTimeInState);
                stateExitTimer.Start();
            }
        }

        public static readonly int UnarnedStandingIdleHash =
            Animator.StringToHash("UnarmedIdle");
        public static readonly int UnarmedLocomotionHash =
            Animator.StringToHash("UnarmedLocomotion");
        public static readonly int Melee2hAttackingHash =
            Animator.StringToHash("Melee2hAttacking");
        public static readonly int AirStateHash =
            Animator.StringToHash("InAirState");
    }

}

