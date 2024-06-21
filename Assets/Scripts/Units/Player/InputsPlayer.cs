using Arcatech.Managers;
using Arcatech.Scenes.Cameras;
using Arcatech.UI;
using KBCore.Refs;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Arcatech.Units.Inputs
{
    [RequireComponent(typeof(AimingComponent),typeof(CostumesControllerComponent))]
    public class InputsPlayer : ControlInputsBase
    {

        [SerializeField, Anywhere] PlayerInputReaderObject _playerInputReader;

        private AimingComponent _aim;
        public AimingComponent Aiming => _aim;


        private IsoCamAdjust _adj;
        private CostumesControllerComponent _costume;
        private GameInterfaceManager _gameInterfaceManager;
        public event SimpleEventsHandler<RuntimeAnimatorController> ChangeLayerEvent;

        #region managedctrl

        public override void StartController()
        {

            base.StartController();

            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;

            _adj ??= new IsoCamAdjust();

            _playerInputReader.EnablePlayerInputs();


            _aim = GetComponent<AimingComponent>();
            _aim.StartController();
            _costume = GetComponent<CostumesControllerComponent>();
            _costume.StartController();

            _playerInputReader.Aim += OnAimAction;
            _playerInputReader.Movement += OnMovementAction;

            _playerInputReader.Melee += OnMeleeAction;
            _playerInputReader.Ranged += OnRangedAction;
            _playerInputReader.Jump += OnJumpAction;

            _playerInputReader.DodgeSpec += OnDodgeSkill;
            _playerInputReader.MeleeSpec += OnMeleeSkill;
            _playerInputReader.RangedSpec += OnRangedSkill;
            _playerInputReader.ShieldSpec += OnShieldSkill;

            _playerInputReader.PausePressed += OnPauseButton;
            _playerInputReader.MountAction += OnMountButton;


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
            DoAiming(delta);
            _aim.UpdateController(delta);
            _costume.UpdateController(delta);

        }
        public override void StopController()
        {
            base.StopController();
            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            // not init for non game levels


            _aim.StopController();
            _costume.StopController();


            _playerInputReader.Aim -= OnAimAction;
            _playerInputReader.Movement -= OnMovementAction;

            _playerInputReader.Melee -= OnMeleeAction;
            _playerInputReader.Ranged -= OnRangedAction;
            _playerInputReader.Jump -= OnJumpAction;

            _playerInputReader.DodgeSpec -= OnDodgeSkill;
            _playerInputReader.MeleeSpec -= OnMeleeSkill;
            _playerInputReader.RangedSpec -= OnRangedSkill;
            _playerInputReader.ShieldSpec -= OnShieldSkill;
            _playerInputReader.PausePressed -= OnPauseButton;
            _playerInputReader.MountAction -= OnMountButton;

            _weaponCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayerWeapon;
            _skillCtrl.SwitchAnimationLayersEvent -= SwitchAnimatorLayerSkill;
            _skillCtrl.SwitchAnimationLayersEvent -= _weaponCtrl.SwitchModels;

        }

        #endregion

        #region controls


        private void OnMountButton()
        {
            // nyi
        }

        private void OnPauseButton()
        {
            if (_gameInterfaceManager == null) return; // weirdstuff happening here as it's called from NULL TODO
            _gameInterfaceManager.OnPauseRequesShowPanelAndPause(true);
        }

        private void OnShieldSkill()
        {            
            RequestCombatAction(UnitActionType.ShieldSkill);
        }
        private void OnRangedSkill()
        {
            RequestCombatAction(UnitActionType.RangedSkill);
        }

        private void OnMeleeSkill()
        {
            RequestCombatAction(UnitActionType.MeleeSkill);
        }

        private void OnJumpAction()
        {
            RequestCombatAction(UnitActionType.Jump);
        }

        private void OnDodgeSkill()
        {
            RequestCombatAction(UnitActionType.DodgeSkill);
        }

        private void OnMovementAction(Vector2 dir)
        {
            AdjustMovementVector(_playerInputReader.InputDirection);
        }

        private void OnAimAction(Vector2 point)
        {
            //throw new System.NotImplementedException();
        }

        private void OnRangedAction()
        {
            RequestCombatAction(UnitActionType.Ranged);
        }

        private void OnMeleeAction()
        {
            RequestCombatAction(UnitActionType.Melee);
        }

        #endregion




        #region anims

        private void SwitchAnimatorLayerSkill(EquipmentType type)
        {
            if (LockInputs) return;
            ChangeLayerEvent?.Invoke(Unit.GetUnitInventory.GetEquipByType(type).AnimatorController);
        }

        private void SwitchAnimatorLayerWeapon(RuntimeAnimatorController ctrl)
        {
            if (LockInputs) return;
            ChangeLayerEvent?.Invoke(ctrl);
        }

        #endregion



        #region movement
        [Header("Movement settings")]


        [SerializeField] float _smoothTime = 0.2f;
        private float _velocity;
        Vector3 _adjustedMovement;

        protected void AdjustMovementVector(Vector2 horizontal)
        {
            Vector3 AD = _adj.Isoright * horizontal.x;
            Vector3 WS = _adj.Isoforward * horizontal.y;
             _adjustedMovement = AD + WS;
        }
        protected override Vector3 DoHorizontalMovement(float delta)
        {
            if (_adjustedMovement.magnitude > zeroF)
            {
                var result = ApplyHorizontalMovement(_adjustedMovement);
                DampAnimatorSpeedFloat(_adjustedMovement.magnitude);
                return result;
            }
            else
            {
                var result = ApplyHorizontalMovement(_adjustedMovement);
                DampAnimatorSpeedFloat(zeroF);
                _rb.velocity = new Vector3(zeroF, _rb.velocity.y, zeroF);
                return result;
            }
        }
        Vector3 ApplyHorizontalMovement(Vector3 adjDir)
        {
            var velocity = adjDir * (_statsCtrl.CurrentStats[BaseStatType.MoveSpeed].GetCurrent * Time.fixedDeltaTime);
            _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);
            return velocity;

        }


        private void DampAnimatorSpeedFloat(float target)
        {
            AnimatorSpeedFloat = Mathf.SmoothDamp(AnimatorSpeedFloat, target, ref _velocity, _smoothTime);
        }


        public float PlayerInputsCross { get; private set; }
        public float PlayerInputsDot { get; private set; }
        public float AnimatorSpeedFloat { get; private set; }
        #endregion




        #region aiming

        [Header("Aiming settings")]
        [SerializeField,Tooltip("If value is less, play rotation animation and rotate player")]
        float _minAngleToPlayRotation = 0.9f;
        public float RotationTreschold => _minAngleToPlayRotation;

        [SerializeField]
        float _dampTime = 0.1f;

        protected Vector3 _sDampVel;
        protected virtual void DoAiming(float delta)
        {
            PlayerInputsDot = _aim.GetDotProduct;
            PlayerInputsCross = _aim.GetRotationToTarget;
            if (_adjustedMovement.magnitude == zeroF)
            {
                if (PlayerInputsDot < _minAngleToPlayRotation)
                {
                    transform.LookAt(Vector3.SmoothDamp(transform.position, _aim.GetLookTarget, ref _sDampVel, _dampTime));
                }
            }
            else
            {
                transform.LookAt(Vector3.SmoothDamp(transform.position, _aim.GetLookTarget, ref _sDampVel, _dampTime));
            }

        }

        #endregion





        protected override void ShieldBreakEventCallback()
        {
            _costume.OnBreak();
            base.ShieldBreakEventCallback();
        }

        protected override void OnLockInputs(bool isLock)
        {
        }

    }
}