using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Triggers;
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
        AimingComponent _aim;
        [SerializeField, Self] protected DashJumpMovementController _movement;

        CostumesControllerComponent costumes;

        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            _inputs.InputsPause += OnInputsPauseButton;
            _aim = (_inputs as InputsPlayer).Aiming;
            costumes = GetComponent<CostumesControllerComponent>();
            costumes.Init(this);

            _movement.speed = movementStats.Stats[Stats.MovementStatType.Movespeed].Max;
            ToggleCamera(true);
        }



        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);

            if (ActionLock || _stunned) return;
            _movement.SetDesiredMoveDirection(_inputs.InputsMovementVector);
            _movement.SetDesiredLookDirection(_inputs.InputsLookVector,_aim.Target!=null);
        }

        protected override void OnActionLock(bool locking)
        {
            // stop moving
            _movement.SetDesiredMoveDirection(Vector3.zero);
        }
        protected override void HandleUnitAction(UnitActionType obj)
        {
            if (UnitPaused || ActionLock || !_movement.isGrounded) return; //add grounded check
            if (obj == UnitActionType.Jump)
            {
                transform.parent = null;
                _animator.SetTrigger("TalkTrigger");
                _movement.DoJump();
                DoActionLogic(movementStats.JumpAction.ProduceAction(this, transform));
            }
            else base.HandleUnitAction(obj);
        }
        public override void ApplyForceResultToUnit(float speed, float distance)
        {
            base.ApplyForceResultToUnit(speed, distance);
            _movement.DisableGroundingOnUnitImpulse(speed, distance);
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
                costumes.OnBreak();
            }
            base.HandleDamage(value);
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
            _movement.SetDesiredMoveDirection(Vector3.zero);
        }

        #endregion
        #region pause
        private void OnInputsPauseButton()
        {
            if (UnitDead) return;
            else
            {
                EventBus<PauseToggleEvent>.Raise(new PauseToggleEvent(!UnitPaused));
            }
        }
        protected override void OnUnitPause(bool isPause)
        {
            // also stop moving
            _movement.SetDesiredMoveDirection(Vector3.zero);
        }


        #endregion

        protected override void HandleInteractionAction(IInteractible i)
        {
            i.AcceptInteraction(this);
        }

    }

}