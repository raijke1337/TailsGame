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
            currentAction?.Update(delta);
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
            if (currentAction!= null && currentAction != act)
            {
                CurrentAction_OnComplete();
            }
            currentAction = act;
            ActionLock = currentAction.LockMovement;
            currentAction.StartAction(this);
            currentAction.OnComplete += CurrentAction_OnComplete;
        }
        private void CurrentAction_OnComplete()
        {
            currentAction.OnComplete -= CurrentAction_OnComplete;
            currentAction = null;
            ActionLock = false;
        }

        protected virtual void HandleUnitAction(UnitActionType obj)
        {
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