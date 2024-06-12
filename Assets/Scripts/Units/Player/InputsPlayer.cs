using Arcatech.Managers;
using Arcatech.Scenes.Cameras;
using Arcatech.UI;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Arcatech.Units.Inputs
{
    [RequireComponent(typeof(AimingComponent),typeof(CostumesControllerComponent))]
    public class InputsPlayer : ControlInputsBase
    {
        private PlayerControls _controls;

        private IsoCamAdjust _adj;
        private AimingComponent _aim;
        public AimingComponent Aiming => _aim;
        private CostumesControllerComponent _costume;


        private GameInterfaceManager _gameInterfaceManager;
        public StaminaController GetComboController => _comboCtrl;
        public CostumesControllerComponent GetCostumesController => _costume;

        public event SimpleEventsHandler<RuntimeAnimatorController> ChangeLayerEvent;

        #region managedctrl

        public override void StartController()
        {

            base.StartController();
            _controls = new PlayerControls();

            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;


            _adj ??= new IsoCamAdjust();


            _controls.Game.Enable();

            _aim = GetComponent<AimingComponent>();
            _aim.StartController();
            _aim.SelectionUpdatedEvent += OnSelectedUpdate;

            _costume = GetComponent<CostumesControllerComponent>();
            _costume.StartController();

            _controls.Game.Dash.performed += Dash_performed;
            _controls.Game.SkillE.performed += SkillE_performed;
            _controls.Game.SkillQ.performed += SkillQ_performed;
            _controls.Game.SkillR.performed += SkillR_performed;
            _controls.Game.MainAttack.performed += MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed += RangedAttack_performed;
            _controls.Game.Pause.performed += Pause_performed;
            _controls.Game.Jump.performed += Jump_performed;

            _weaponCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayerWeapon;
            _skillCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayerSkill;
            _skillCtrl.SwitchAnimationLayersEvent += _weaponCtrl.SwitchModels;
            transform.LookAt(transform.forward);

            _gameInterfaceManager = GameManager.Instance.GetGameControllers.GameInterfaceManager;
        }

        public override void UpdateController(float delta)
        {
            if (_aim == null) return;

            base.UpdateController(delta);
            _aim.UpdateController(delta);
            _costume.UpdateController(delta);
        }

        public override void StopController()
        {
            base.StopController();
            _controls.Game.Disable();

            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            // not init for non game levels
            _aim.StopController();
            _costume.StopController();

            _controls.Game.Dash.performed -= Dash_performed;
            _controls.Game.SkillE.performed -= SkillE_performed;
            _controls.Game.SkillQ.performed -= SkillQ_performed;
            _controls.Game.SkillR.performed -= SkillR_performed;
            _controls.Game.MainAttack.performed -= MeleeAttack_performed;
            _controls.Game.SpecialAttack.performed -= RangedAttack_performed;
            _aim.SelectionUpdatedEvent -= OnSelectedUpdate;
            _controls.Game.Pause.performed -= Pause_performed;
            _controls.Game.Jump.performed -= Jump_performed;

            _weaponCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayerWeapon;
            _skillCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayerSkill;
            _skillCtrl.SwitchAnimationLayersEvent -= _weaponCtrl.SwitchModels;

        }

        #endregion



        private void SwitchAnimatorLayerSkill(EquipItemType type)
        {
            if (LockInputs) return;
            ChangeLayerEvent?.Invoke(Unit.GetUnitInventory.GetEquipByType(type).AnimatorController);
        }

        private void SwitchAnimatorLayerWeapon(RuntimeAnimatorController ctrl)
        {
            if (LockInputs) return;
            ChangeLayerEvent?.Invoke(ctrl);
        }

        private void Pause_performed(CallbackContext obj)
        {
            if (_gameInterfaceManager == null) return; // weirdstuff happening here as it's called from NULL TODO

            _gameInterfaceManager.OnPauseRequesShowPanelAndPause(true);
        }

        #region combat actions

        protected void RangedAttack_performed(CallbackContext obj)
        {
            DoCombatAction(CombatActionType.Ranged);
        }
        protected void MeleeAttack_performed(CallbackContext obj)
        {
            DoCombatAction(CombatActionType.Melee);
        }
        private void Dash_performed(CallbackContext obj)
        {
            if (_inputsMovement == Vector3.zero) return;
            DoCombatAction(CombatActionType.Dodge);
        }
        protected void SkillR_performed(CallbackContext obj)
        {
            DoCombatAction(CombatActionType.ShieldSpecialR);
        }
        protected void SkillQ_performed(CallbackContext obj)
        {
            DoCombatAction(CombatActionType.MeleeSpecialQ);
        }
        protected void SkillE_performed(CallbackContext obj)
        {
            DoCombatAction(CombatActionType.RangedSpecialE);
        }
        private void Jump_performed(CallbackContext obj)
        {
            DoJumpAction();
        }

        #endregion

        #region movement vars
        protected override void SetAimDirection()
        {
            _inputsAiming = _aim.GetLookTarget;
        }
        protected override void SetMoveDirection()
        {
            Vector2 control = _controls.Game.WASD.ReadValue<Vector2>();
            Vector3 AD = _adj.Isoright * control.x;
            Vector3 WS = _adj.Isoforward * control.y;
            _inputsMovement = AD + WS;
        }

        protected override void SetRotationDot()
        {
            _inputsDot = _aim.GetDotProduct;
        }

        protected override void SetRotationCross()
        {
            _inputsCross = _aim.GetRotationToTarget;
        }

        #endregion
        #region gizmo

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + GetMoveDirection);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.forward + transform.position);
        }


        #endregion

        private void OnSelectedUpdate(bool isSelect, BaseTargetableItem item)
        {
            _gameInterfaceManager.OnPlayerSelectedTargetable(item, isSelect);
        }

        protected override void ShieldBreakEventCallback()
        {
            _costume.OnBreak();
            base.ShieldBreakEventCallback();
        }


    }
}