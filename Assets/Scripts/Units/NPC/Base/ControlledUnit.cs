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
        [SerializeField, Self] protected MovementControllerComponent _movement;
        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
          
            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                LockUnit = false;
                
            }
            _inputs.UnitActionRequestedEvent += HandleUnitAction;
        }
        public override void DisableUnit()
        {
            base.DisableUnit();
            _inputs.UnitActionRequestedEvent -= HandleUnitAction;
        }

        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);

            currentAction?.Update(delta);
            if (_lock) return;

            _movement.SetMoveDirection(_inputs.InputsMovementVector);
            _movement.LookDirection = (_inputs.InputsLookVector);
        }

        bool _lock;
        protected bool LockMovement
        {
            get => _lock;
            set
            {
                if (value)
                {
                    _movement.SetMoveDirection(Vector3.zero);
                }
                _lock = value;
            }
        }


        #region actions

        BaseUnitAction currentAction;
        void DoActionLogic(BaseUnitAction act)
        {
            if (currentAction != null)
            {
                // case - combo
                currentAction.OnComplete -= CurrentAction_OnComplete;
            }

            currentAction = act;
            LockMovement = currentAction.LocksInputs;
            currentAction.DoAction(this);
            currentAction.OnComplete += CurrentAction_OnComplete;
        }

        private void CurrentAction_OnComplete()
        {
            currentAction.OnComplete -= CurrentAction_OnComplete;
            currentAction = null;
            _lock = false;
        }

        protected virtual void HandleUnitAction(UnitActionType obj)
        {
            BaseUnitAction a;
            switch (obj)
            {
                case UnitActionType.Jump:
                    transform.parent = null;
                    _movement.DoJump();
                    DoActionLogic(movementStats.JumpAction.ProduceAction(this));                    
                    break;
                case UnitActionType.Melee:
                    if (_weapons.TryUseAction(obj, out a)) DoActionLogic(a);
                    break;
                case UnitActionType.Ranged:
                    if (_weapons.TryUseAction(obj, out a)) DoActionLogic(a);
                    break;
                case UnitActionType.DodgeSkill:
                    if (_skills.TryUseAction(obj, out a)) DoActionLogic(a);
                    break;
                case UnitActionType.MeleeSkill:
                    if (_skills.TryUseAction(obj, out a)) DoActionLogic(a);
                    break;
                case UnitActionType.RangedSkill:
                    if (_skills.TryUseAction(obj, out a)) DoActionLogic(a);
                    break;
                case UnitActionType.ShieldSkill:
                    if (_skills.TryUseAction(obj, out a)) DoActionLogic(a);
                    break;
                case UnitActionType.None:
                    Debug.LogError($"action type not set for something in {this}");
                    break;
            }

            
            #endregion

        }
    }
}