using Arcatech.Managers;
using Arcatech.Units.Stats;
using KBCore.Refs;
using UnityEngine;
using Arcatech.StateMachine;
using ECM.Components;
using ECM.Common;
namespace Arcatech.Units
{
    [RequireComponent(typeof(ControlInputsBase),typeof(Rigidbody))]
    public abstract class ControlledUnit : ArmedUnit
    {
        [SerializeField,Header("Movement")] protected MovementStatsConfig movementStats;
        [SerializeField,Self] protected CharacterMovement movement;
        [SerializeField,Self] protected Rigidbody _rb;


        [Space, Self, SerializeField] protected ControlInputsBase _inputs;
        [SerializeField] protected ArcaStateMachine UnitStateMachine;

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
          
            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                LockUnit = false;

                SetupStateMachine();
            }
            _inputs.UnitActionRequestedEvent += HandleUnitAction;
        }
        public override void DisableUnit()
        {
            base.DisableUnit();
            _inputs.UnitActionRequestedEvent -= HandleUnitAction;
        }
        protected abstract void SetupStateMachine();

        public override void RunFixedUpdate(float delta)
        {
            base.RunFixedUpdate(delta);
            UnitStateMachine.Update(delta);
        }
        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            UnitStateMachine.FixedUpdate(delta);
        }
        protected void HandleUnitAction(UnitActionType obj)
        {
            switch (obj)
            {
                case UnitActionType.Melee:
                    if (_weapons.TryUseAction(obj))
                    {
                        Debug.Log($"{obj} OK");
                    }
                    break;
                case UnitActionType.Ranged:
                    if (_weapons.TryUseAction(obj))
                    {
                        Debug.Log($"{obj} OK");
                    }
                    break;
                case UnitActionType.DodgeSkill:
                    if (_skills.TryUseAction(obj))
                    {
                        Debug.Log($"{obj} OK");
                    }
                    break;
                case UnitActionType.MeleeSkill:
                    if (_skills.TryUseAction(obj))
                    {
                        Debug.Log($"{obj} OK");
                    }
                    break;
                case UnitActionType.RangedSkill:
                    if (_skills.TryUseAction(obj))
                    {
                        Debug.Log($"{obj} OK");
                    }
                    break;
                case UnitActionType.ShieldSkill:
                    if (_skills.TryUseAction(obj))
                    {
                        Debug.Log($"{obj} OK");
                    }
                    break;
                case UnitActionType.Jump:
                    jumpPressed = true;
                    break;
            }
        }
        protected override void OnLockUnit(bool locked)
        {
            movement.Pause(locked);
        }
        public void DoRotationInDeltaTime()
        {
            movement.Rotate(_inputs.InputsLookVector, movementStats.Stats[MovementStatType.TurnSpeed].Start, true);
        }

        public void DoMovementInFixedTime()
        {
            var desiredVelocity = _inputs.InputsMovementVector.clamped(out _) * 100f;
            movement.Move(desiredVelocity, movementStats.Stats[MovementStatType.MoveSpeed].Max);
        
        }
        public void DoMidAirMovement()
        {
            
        }
        protected bool jumpPressed;
        protected void StartJump()
        {
            movement.ApplyVerticalImpulse(jumpImpulse);
            movement.DisableGrounding();
        }

        #region jumping

        float jumpImpulse
        {
            get { return Mathf.Sqrt(2.0f * movementStats.Stats[MovementStatType.JumpForce].Start * movement.gravity.magnitude); }
        }

        #endregion

    }
}