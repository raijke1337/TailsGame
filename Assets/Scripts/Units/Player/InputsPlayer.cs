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
        public ComboController GetComboController => _comboCtrl;
        public CostumesControllerComponent GetCostumesController => _costume;

        public event SimpleEventsHandler<EquipItemType> ChangeLayerEvent;


        // used to adjust the raycast plane for vertical movement
        private float lastY;



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

            _weaponCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent += SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent += _weaponCtrl.SwitchModels;
            transform.LookAt(transform.forward);

            _gameInterfaceManager = GameManager.Instance.GetGameControllers.GameInterfaceManager;
            lastY = Unit.transform.position.y;
        }

        

        private void Pause_performed(CallbackContext obj)
        {
            if (_gameInterfaceManager == null) return; // weirdstuff happening here as it's called from NULL TODO

            _gameInterfaceManager.OnPauseRequesShowPanelAndPause(true);

        }

        public override void UpdateController(float delta)
        {
            base.UpdateController(delta);
            if ( _aim == null) return;
            _aim.UpdateController(delta);
            _costume.UpdateController(delta);

            CalculateMovement();
            RotateToAim();
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

            _weaponCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayer;
            _skillCtrl.SwitchAnimationLayersEvent -= _weaponCtrl.SwitchModels;

        }

        #endregion


        private void SwitchAnimatorLayer(EquipItemType type)
        {
            if (LockInputs) return;
            ChangeLayerEvent?.Invoke(type);
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
            if (MoveDirectionFromInputs == Vector3.zero) return;
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

        #region movement
        private void CalculateMovement()
        {

            if (LockInputs) return;
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

            float currY = Unit.transform.position.y;
            if (lastY != currY)
            {
                float delta = lastY - currY;
                lastY = Unit.transform.position.y;
                _aim.OnVerticalAdjust(delta);
            }

        }
        private void RotateToAim()
        {
            if (_aim == null || InputDirectionOverride != Vector3.zero) return;
            LerpRotateToTarget(_aim.GetLookPoint, lastDelta);

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