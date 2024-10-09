using Arcatech.Managers;
using Arcatech.Triggers;
using Arcatech.Units.Stats;
using KBCore.Refs;
using System.Collections;
using UnityEngine;
namespace Arcatech.Units
{
    [RequireComponent(typeof(ControlInputsBase), typeof(Rigidbody))]
    public abstract class ControlledUnit : ArmedUnit, IInteractor
    {
        [Space, Header("Controlled Unit")]
        [SerializeField] protected MovementStatsConfig movementStats;
        [SerializeField, Self] protected Rigidbody _rb;
        [Self, SerializeField] protected ControlInputsBase _inputs;
        [SerializeField, Range(0, 300)] protected float stunEndStamina = 30f;
        [SerializeField, Range(0.01f, 1)] protected float stunEndGetUpTime = 0.5f;

        protected bool _stunned = false;
        Coroutine stunEndProgress;


        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
          
            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                UnitPaused = false;                
            }
            _inputs.UnitActionRequestedEvent += HandleUnitAction;
            _inputs.RequestInteraction += HandleInteractionAction;

            stunEndStamina = Mathf.Clamp(stunEndStamina,_stats.GetStatValue(BaseStatType.Stamina).GetMin, _stats.GetStatValue(BaseStatType.Stamina).GetMax);
        }

        protected abstract void HandleInteractionAction(IInteractible i);

        public override void DisableUnit()
        {
            base.DisableUnit();
            _inputs.UnitActionRequestedEvent -= HandleUnitAction;
        }

        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            if (_stunned)
            {
                if (_stats.GetStatValue(BaseStatType.Stamina).GetCurrent >= stunEndStamina && stunEndProgress == null)
                {
                    stunEndProgress = StartCoroutine(StunCancelCoroutine());
                }
                else return;
            }
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
        IEnumerator StunCancelCoroutine()
        {
            yield return new WaitForSeconds(stunEndGetUpTime);
            _stunned = false;
            _animator.SetTrigger("StunEnd");
            ActionLock = false;
            stunEndProgress = null;
            yield return null;
        }

        bool _lockAction;
        protected bool ActionLock
        {
            get => _lockAction;
            set
            {
                OnActionLock(value);
                _lockAction = value;
            }
        }
        protected abstract void OnActionLock(bool locking);

        protected override void HandleStun()
        {
            base.HandleStun();
            ActionLock = true;
            _stunned = true;
        }

        #region actions

        protected BaseUnitAction currentAction;

        protected override void OnForceAction(BaseUnitAction act)
        {
            base.OnForceAction(act);
            DoActionLogic(act);
        }

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
        public bool CanDoAction(UnitActionType action)
        {
            if (!_ground.isOnGround) return false;
            else switch (action)
                {
                    case UnitActionType.Melee:
                        return _weapons.CanUseAction(action);
                    case UnitActionType.Ranged:
                        return _weapons.CanUseAction(action);
                    case UnitActionType.DodgeSkill:
                        return _skills.CanUseAction(action);
                    case UnitActionType.MeleeSkill:
                        return _skills.CanUseAction(action);
                    case UnitActionType.RangedSkill:
                        return _skills.CanUseAction(action);
                    case UnitActionType.ShieldSkill:
                        return _skills.CanUseAction(action);
                    default:
                        return false;
                }

        }
        protected virtual void HandleUnitAction(UnitActionType obj)
        {
            // this execution is blocked by ActionLock bool
            BaseUnitAction a;

            if (!_ground.isOnGround) return;
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
        protected override void HandleDeath()
        {
            currentAction?.CompleteAction();
            base.HandleDeath();
        }

        public virtual void ReceiveInteraction(IInteractible interactible)
        {
            if (_showDebugs) Debug.Log($"NYI: {this} receives interaction from {interactible}");
        }
    }
}