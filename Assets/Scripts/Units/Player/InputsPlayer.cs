using System.Reflection;
using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Arcatech.Managers;
using Arcatech.UI;

namespace Arcatech.Units.Inputs
{
    [RequireComponent(typeof(AimingComponent))]
    public class InputsPlayer : ControlInputsBase
    {
        private PlayerControls _controls;

        private IsoCamAdjust _adj;
        private AimingComponent _aim;

        private GameInterfaceManager _gameInterfaceManager;
        public ComboController GetComboController => _comboCtrl;

        public event SimpleEventsHandler<EquipItemType> ChangeLayerEvent;
        public bool IsInMeleeCombo = false;

        #region managedctrl

        public override void StartController()
        {
            base.StartController();

            if (GameManager.Instance.GetCurrentLevelData.Type != LevelType.Game) return;
            _adj ??= new IsoCamAdjust();

            _controls = new PlayerControls();
            _controls.Game.Enable();
            _aim = GetComponent<AimingComponent>();
            _aim.StartController();
            _aim.SelectionUpdatedEvent += OnSelectedUpdate;
            _controls.Game.Dash.performed += Dash_performed;
            _controls.Game.SkillE.performed += SkillE_performed;
            _controls.Game.SkillQ.performed += SkillQ_performed;
            _controls.Game.SkillR.performed += SkillR_performed;
            _controls.Game.MainAttack.performed += MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed += RangedAttack_performed;
            _controls.Game.Pause.performed += Pause_performed;

            _weaponCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent += _weaponCtrl.SwitchModels;
            transform.LookAt(transform.forward);

            _gameInterfaceManager = GameManager.Instance.GetGameControllers.GameInterfaceManager;
        }

        private void Pause_performed(CallbackContext obj)
        {
            _gameInterfaceManager.OnPauseRequest(true);

        }

        public override void UpdateController(float delta)
        {
            base.UpdateController(delta);
            if (IsInputsLocked || _aim == null) return; // set during dodges and attack motions
            _aim.UpdateController(delta);
            CalculateMovement();
            RotateToAim();
        }

        public override void StopController()
        {
            base.StopController();
            _controls.Game.Disable();
            _aim.StopController();
            _controls.Game.Dash.performed -= Dash_performed;
            _controls.Game.SkillE.performed -= SkillE_performed;
            _controls.Game.SkillQ.performed -= SkillQ_performed;
            _controls.Game.SkillR.performed -= SkillR_performed;
            _controls.Game.MainAttack.performed -= MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed -= RangedAttack_performed;
            _aim.SelectionUpdatedEvent -= OnSelectedUpdate;
            _controls.Game.Pause.performed -= Pause_performed;

            _weaponCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent -= _weaponCtrl.SwitchModels;
        }

        #endregion


        private void SwitchAnimatorLayer(EquipItemType type)
        {
            if (IsInputsLocked) return;
            ChangeLayerEvent?.Invoke(type);
        }


        #region controller checks

        protected void RangedAttack_performed(CallbackContext obj)
        {
            if (IsInputsLocked) return;
            if (_weaponCtrl.OnWeaponUseSuccessCheck(EquipItemType.RangedWeap))
                CombatActionSuccessCallback(CombatActionType.Ranged);

        }
        protected void MeleeAttack_performed(CallbackContext obj)
        {
            if (IsInputsLocked && !IsInMeleeCombo) return;
            if (_weaponCtrl.OnWeaponUseSuccessCheck(EquipItemType.MeleeWeap))
                CombatActionSuccessCallback(CombatActionType.Melee);
            
        }
        protected void SkillR_performed(CallbackContext obj)
        {
            if (IsInputsLocked) return;
            if (_skillCtrl.TryUseSkill(CombatActionType.ShieldSpecialR, _comboCtrl.GetAvailableCombo.GetCurrent, out var sk))
            {
                _comboCtrl.UseCombo(sk.Data.Cost);
                SkillSpawnEventCallback(sk);
                CombatActionSuccessCallback(CombatActionType.ShieldSpecialR);
            }
        }
        protected void SkillQ_performed(CallbackContext obj)
        {
            if (IsInputsLocked) return;
            if (_skillCtrl.TryUseSkill(CombatActionType.MeleeSpecialQ, _comboCtrl.GetAvailableCombo.GetCurrent, out var sk))
            {
                _comboCtrl.UseCombo(sk.Data.Cost);
                SkillSpawnEventCallback(sk);
                CombatActionSuccessCallback(CombatActionType.MeleeSpecialQ);
            }
        }
        protected void SkillE_performed(CallbackContext obj)
        {
            if (IsInputsLocked) return;
            if (_skillCtrl.TryUseSkill(CombatActionType.RangedSpecialE, _comboCtrl.GetAvailableCombo.GetCurrent, out var sk))
            {
                _comboCtrl.UseCombo(sk.Data.Cost);
                SkillSpawnEventCallback(sk);    
                CombatActionSuccessCallback(CombatActionType.RangedSpecialE);
            }
        }
        private void Dash_performed(CallbackContext obj)
        {
            if (IsInputsLocked || MoveDirectionFromInputs == Vector3.zero) return;
            //can't dash from standing
            if (_skillCtrl.TryUseSkill(CombatActionType.Dodge, _comboCtrl.GetAvailableCombo.GetCurrent, out var sk))
            {
                _comboCtrl.UseCombo(sk.Data.Cost);
                SkillSpawnEventCallback(sk);
                CombatActionSuccessCallback(CombatActionType.Dodge);
            }

            // this is the old dash, non-skill
            //if (_dodgeCtrl.IsDodgePossibleCheck())
            //{
            //    CombatActionSuccessCallback(CombatActionType.Dodge);
            //}
        }
        #endregion


        private void CalculateMovement()
        {

            if (IsInputsLocked) return;
            Vector2 input;
            if (InputDirectionOverride != Vector3.zero)
            {
                input = InputDirectionOverride;
            }
            else
            {
                if (_controls == null) return; //shouldn happen
                input = _controls.Game.WASD.ReadValue<Vector2>();
            }
            Vector3 AD = _adj.Isoright * input.x;
            Vector3 WS = _adj.Isoforward * input.y;
            MoveDirectionFromInputs = AD + WS;
        }
        private void RotateToAim()
        {
            if (_aim == null || InputDirectionOverride != Vector3.zero) return;
            LerpRotateToTarget(_aim.GetLookPoint, lastDelta);

        }


        protected override void LerpRotateToTarget(Vector3 looktarget, float delta) // here we also apply turning animations
        {
            base.LerpRotateToTarget(looktarget, delta);
            // TODO Detect rotation to play animations
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + GetMoveDirection);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.forward + transform.position);

        }

        private void OnSelectedUpdate(bool isSelect,BaseTargetableItem item)
        {
            _gameInterfaceManager.OnPlayerSelectedTargetable(item, isSelect);
        }


        public override ReferenceUnitType GetUnitType()
        {
            return ReferenceUnitType.Player;
        }

    }
}