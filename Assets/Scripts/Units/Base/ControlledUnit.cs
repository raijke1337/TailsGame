using Arcatech.Managers;
using Arcatech.Units.Stats;
using KBCore.Refs;
using UnityEngine;
namespace Arcatech.Units
{
    [RequireComponent(typeof(ControlInputsBase), typeof(Rigidbody))]
    public abstract class ControlledUnit : ArmedUnit
    {
        [Space, Header("Controlled Unit")]
        [SerializeField] protected MovementStatsConfig movementStats;
        [SerializeField, Self] protected Rigidbody _rb;
        [Self, SerializeField] protected ControlInputsBase _inputs;

        protected bool UnitIsGrounded { get;set;}

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
          
            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                UnitPaused = false;                
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
            if (currentAction != null)
            {
                switch (currentAction?.UpdateAction(delta))
                {
                    case UnitActionState.None:
                        break;
                    case UnitActionState.Started:
                        ActionLock = currentAction.LockMovement;
                        break;
                    case UnitActionState.ExitTime:
                        ActionLock =false;
                        break;
                    case UnitActionState.Completed:
                        ActionLock = false;
                        break;
                }
            }
        }

        bool _lockAction;
        protected bool ActionLock
        {
            get => _lockAction;
            set
            {
                if (value)
                {
                    OnActionLock(value);
                }
                _lockAction = value;
            }
        }
        protected abstract void OnActionLock(bool locking);


        #region actions

        protected BaseUnitAction currentAction;
        protected void DoActionLogic(BaseUnitAction act)
        {
            if (currentAction!= null && currentAction != act && currentAction.GetActionState != UnitActionState.Completed)
            {
                currentAction.CompleteAction();
            }
            currentAction = act;
            ActionLock = currentAction.LockMovement;
            currentAction.StartAction();
        }

        protected virtual void HandleUnitAction(UnitActionType obj)
        {
            // this execution is blocked by ActionLock bool
            BaseUnitAction a;

            if (!UnitIsGrounded) return;
            switch (obj)
            {
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
                default:
                    Debug.LogWarning($"action type {obj} not supported in {this}");
                    break;
            }

            
            #endregion

        }
    }
}