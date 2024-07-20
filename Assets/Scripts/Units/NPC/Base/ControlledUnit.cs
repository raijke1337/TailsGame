using Arcatech.Managers;
using Arcatech.Units.Stats;
using KBCore.Refs;
using UnityEngine;
namespace Arcatech.Units
{
    [RequireComponent(typeof(ControlInputsBase),typeof(Rigidbody))]
    public abstract class ControlledUnit : ArmedUnit
    {

        [SerializeField,Self] protected Rigidbody _rb;
        [SerializeField, Header("Inputs")] 
        protected MovementStatsConfig movementStats;
        [Self, SerializeField] protected ControlInputsBase _inputs;
        [SerializeField] protected ActionsHandler _unitActions;
        [SerializeField, Self] protected MovementControllerComponent _movement;
        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
          
            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                LockUnit = false;
                
            }
            _unitActions = new ActionsHandler(this, movementStats.JumpAction);
            _inputs.UnitActionRequestedEvent += HandleUnitAction;
        }
        public override void DisableUnit()
        {
            base.DisableUnit();
            _inputs.UnitActionRequestedEvent -= HandleUnitAction;
        }
        // protected abstract void SetupStateMachine();

        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            _movement.SetMoveDirection(_inputs.InputsMovementVector);
            _movement.LookDirection = (_inputs.InputsLookVector);
        }



        protected virtual void HandleUnitAction(UnitActionType obj)
        {
            switch (obj)
            {
                case UnitActionType.Jump:
                    _movement.DoJump();
                    _unitActions.OnCommand(obj);
                    break;
                default:
                    _unitActions.OnCommand(obj);
                    break;
            }
        }
    }
}