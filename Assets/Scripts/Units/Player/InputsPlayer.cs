using System.Reflection;
using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Arcatech.Managers;

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

            _weaponCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent += _weaponCtrl.SwitchModels;
            transform.LookAt(transform.forward);

            _gameInterfaceManager = GameManager.Instance.GetGameControllers.GameInterfaceManager;
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
            if (_weaponCtrl.OnWeaponUseSuccessCheck(EquipItemType.RangedWeap, out string text))
                CombatActionSuccessCallback(CombatActionType.Ranged);
            else Debug.Log(text);

        }
        protected void MeleeAttack_performed(CallbackContext obj)
        {
            if (IsInputsLocked && !IsInMeleeCombo) return;
            if (_weaponCtrl.OnWeaponUseSuccessCheck(EquipItemType.MeleeWeap, out string text))
                CombatActionSuccessCallback(CombatActionType.Melee);
            else Debug.Log(text);
        }
        protected void SkillR_performed(CallbackContext obj)
        {
            if (IsInputsLocked) return;
            if (_skillCtrl.RequestSkill(CombatActionType.ShieldSpecialR, out var c))
            {
                if (_comboCtrl.UseCombo(c))
                    CombatActionSuccessCallback(CombatActionType.ShieldSpecialR);
            }
        }
        protected void SkillQ_performed(CallbackContext obj)
        {
            if (IsInputsLocked) return;
            if (_skillCtrl.RequestSkill(CombatActionType.MeleeSpecialQ, out var c))
            {
                if (_comboCtrl.UseCombo(c))
                    CombatActionSuccessCallback(CombatActionType.MeleeSpecialQ);
            }
        }
        protected void SkillE_performed(CallbackContext obj)
        {
            if (IsInputsLocked) return;
            if (_skillCtrl.RequestSkill(CombatActionType.RangedSpecialE, out var c))
            {
                if (_comboCtrl.UseCombo(c))
                    CombatActionSuccessCallback(CombatActionType.RangedSpecialE);
            }
        }
        private void Dash_performed(CallbackContext obj)
        {
            if (IsInputsLocked) return;
            if (MoveDirectionFromInputs == Vector3.zero) return; //can't dash from standing
            if (_dodgeCtrl.IsDodgePossibleCheck())
            {
                CombatActionSuccessCallback(CombatActionType.Dodge);
            }
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
            // TODO Detect rotation
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + GetMoveDirection);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.forward + transform.position);

        }

        private void OnSelectedUpdate(bool isSelect)
        {
            _gameInterfaceManager.UpdateSelected(_aim.GetSelectableItem, isSelect);
        }


        public override ReferenceUnitType GetUnitType()
        {
            return ReferenceUnitType.Player;
        }

    }
}