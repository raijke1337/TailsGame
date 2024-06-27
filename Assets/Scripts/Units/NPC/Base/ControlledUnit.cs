using Arcatech.Managers;
using Arcatech.Units.Stats;
using KBCore.Refs;
using UnityEngine;

namespace Arcatech.Units
{
    [RequireComponent(typeof(ControlInputsBase))]
    public abstract class ControlledUnit : ArmedUnit
    {
        [SerializeField] protected MovementStatsConfig movementStats;

        [SerializeField, Self] protected ControlInputsBase _inputs;

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
        }
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
        }
        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            _inputs.ControllerUpdate(delta);

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

    }


}