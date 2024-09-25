using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Units.Inputs;
using KBCore.Refs;
using UnityEngine;


namespace Arcatech.Units
{
    public class PlayerUnit : ControlledUnit
    {
        [Space, Header("Player Unit")]
        [SerializeField] int _armorBreakStam = 30;
        [SerializeField] int _armorBreakEnergy = 30;

        [SerializeField, Child] protected Camera _faceCam;
        public AimingComponent GetAimingComponent => (_inputs as InputsPlayer).Aiming;

        [SerializeField, Self] protected PlayerMovementController _movement;


        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            ToggleCamera(true);
        }

        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            UnitIsGrounded = _movement.isGrounded;

            if (ActionLock) return;
            _movement.SetDesiredMoveDirection(_inputs.InputsMovementVector);
            _movement.SetDesiredLookDirection(_inputs.InputsLookVector);
        }
        //doaction in applyforce leads to this
        public override void ApplyForceResultToUnit(float impulse, float time)
        {
            _movement.ApplyPhysicalMovementResult(impulse, time);
        }

        protected override void OnActionLock(bool locking)
        {
            // stop moving
            _movement.SetDesiredMoveDirection(Vector3.zero);
        }
        protected override void HandleUnitAction(UnitActionType obj)
        {
            if (obj == UnitActionType.Jump)
            {
                transform.parent = null;
                _movement.DoJump();
                DoActionLogic(movementStats.JumpAction.ProduceAction(this, transform));
            }
            else base.HandleUnitAction(obj);
        }

        #region inventory

        protected override UnitInventoryItemConfigsContainer SelectSerializedItemsConfig()
        {

            if (DataManager.Instance.IsNewGame)
            {
                return new UnitInventoryItemConfigsContainer(defaultEquips);
            }
            else
            {
                return DataManager.Instance.GetPlayerSaveEquips;
            }

        }
        #endregion

        #region stats
        protected override void UpdateStats()
        {
            base.UpdateStats();
            foreach (var pair in _stats.GetStatValues)
            {
                EventBus<PlayerStatsChangedUIEvent>.Raise(new PlayerStatsChangedUIEvent(pair.Key,pair.Value));
            }
             // used by player UI
        }

        protected override void HandleDamage(float value)
        {
            UpdateStats();
            if (_stats.GetStatValue(BaseStatType.Stamina).GetCurrent <= _armorBreakStam && _stats.GetStatValue(BaseStatType.Energy).GetCurrent <= _armorBreakEnergy)
            {
                if (_showDebugs) Debug.Log($"Armor break!");
                CostumesControllerComponent.instance.OnBreak();
            }
            base.HandleDamage(value);
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
            _movement.SetDesiredMoveDirection(Vector3.zero);
        }

        #endregion




    }

}