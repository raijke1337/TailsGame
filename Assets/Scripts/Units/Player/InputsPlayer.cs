using Arcatech.EventBus;
using Arcatech.Managers;
using Arcatech.Scenes.Cameras;
using Arcatech.Triggers;
using KBCore.Refs;
using UnityEngine;

namespace Arcatech.Units.Inputs
{
    [RequireComponent(typeof(AimingComponent))]
    public class InputsPlayer : ControlInputsBase
    {
        [Space,Header("Player inputs")]
        [SerializeField, Anywhere] PlayerInputReaderObject _playerInputReader;
        [SerializeField,Self] private AimingComponent _aim;
        [SerializeField] float _interactRange = 3f;
        public AimingComponent Aiming => _aim;
        private IsoCamAdjust _adj;

        #region managedctrl

        protected override void OnEnable()
        {
            base.OnEnable();
            _adj ??= new IsoCamAdjust();

            _playerInputReader.EnablePlayerInputs();
            _aim = GetComponent<AimingComponent>();
            _aim?.StartController();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _aim?.StopController();
        }
        private void Update()
        {
            _aim?.ControllerUpdate(Time.deltaTime);
        }
        #endregion


       #region inputs section

        protected override ControlInputsBase ControllerBindings(bool start)
        {
            if (start)

            {
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
            }
            else
            {


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
            }

            return this;
        }
        #endregion

        #region controls handling


        private void OnMountButton()
        {
            if (Aiming.CheckInteractive(out var inter))
            {
                if (Vector3.Distance(transform.position,inter.Position)<= _interactRange)
                {
                    CallBackInteraction(inter);
                }
                else
                {
                    Debug.Log($"Too far to interact with {inter}");
                }
            }
        }
        private void OnPauseButton()
        {
            CallBackPause();
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

            Vector3 AD = _adj.Isoright * _playerInputReader.InputDirection.x;
            Vector3 WS = _adj.Isoforward * _playerInputReader.InputDirection.z;

            InputsMovementVector = AD + WS;
        }

        private void OnAimAction(Vector2 point)
        {
            InputsLookVector = _aim.GetDirectionToTarget;
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

    }
}