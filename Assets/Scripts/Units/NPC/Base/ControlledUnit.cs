using Arcatech.Managers;
using Arcatech.Units.Stats;
using KBCore.Refs;
using UnityEngine;
using Arcatech.StateMachine;
namespace Arcatech.Units
{
    [RequireComponent(typeof(ControlInputsBase))]
    public abstract class ControlledUnit : ArmedUnit
    {
        [SerializeField] protected MovementStatsConfig movementStats;
        [SerializeField, Self] protected ControlInputsBase _inputs;

        #region state machine
        [SerializeField] protected ArcaStateMachine UnitStateMachine;
        public abstract void DoHorizontalMovement(float delta);
        public abstract void DoRotation(float delta);

        #endregion




        public override void StartControllerUnit()
        {
            base.StartControllerUnit();

            _inputs.PopulateDictionary().
                SetMovementStats(movementStats)
                .StartController();
                _inputs.UnitActionRequestedEvent += HandleUnitAction;             
            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                LockUnit = false;
            }
            SetupStateMachine();
        }

        protected abstract void SetupStateMachine();

        public override void DisableUnit()
        {
            base.DisableUnit();
            _inputs.StopController();
            _inputs.UnitActionRequestedEvent -= HandleUnitAction;
        }

        public override void RunFixedUpdate(float delta)
        {
            base.RunFixedUpdate(delta);
            _inputs.FixedControllerUpdate(delta);
            UnitStateMachine.Update(delta);
        }
        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            _inputs.ControllerUpdate(delta);
            UnitStateMachine.FixedUpdate(delta);
        }
        protected void HandleUnitAction(UnitActionType obj)
        {
            switch (obj)
            {
                case UnitActionType.Melee:
                    if (_weapons.TryUseAction(obj))
                    {
                        _animator.SetTrigger("MeleeAttack");
                    }
                    break;
                case UnitActionType.Ranged:
                    if (_weapons.TryUseAction(obj))
                    {
                        _animator.SetTrigger("RangedAttack");
                    }
                    break;
                case UnitActionType.DodgeSkill:
                    if (_skills.TryUseAction(obj))
                    {
                        _animator.SetTrigger("Dodge");
                    }
                    break;
                case UnitActionType.MeleeSkill:
                    if (_skills.TryUseAction(obj))
                    {
                        _animator.SetTrigger("MeleeSpecial");
                    }
                    break;
                case UnitActionType.RangedSkill:
                    if (_skills.TryUseAction(obj))
                    {
                        _animator.SetTrigger("RangedSpecial");
                    }
                    break;
                case UnitActionType.ShieldSkill:
                    if (_skills.TryUseAction(obj))
                    {
                        _animator.SetTrigger("ShieldSpecial");
                    }
                    break;
                case UnitActionType.Jump:
                    break;
            }
        }


        protected abstract bool IdleConditions();
        protected abstract bool UnitInAir();


    
    
    
    }


}