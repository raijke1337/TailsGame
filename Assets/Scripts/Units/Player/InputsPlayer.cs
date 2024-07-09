using Arcatech.Managers;
using Arcatech.Scenes.Cameras;
using KBCore.Refs;
using UnityEngine;

namespace Arcatech.Units.Inputs
{
    [RequireComponent(typeof(AimingComponent),typeof(CostumesControllerComponent))]
    public class InputsPlayer : ControlInputsBase
    {

        [SerializeField, Anywhere] PlayerInputReaderObject _playerInputReader;
        public Vector2 InputVector => _playerInputReader.InputDirection;

        [SerializeField,Self] private AimingComponent _aim;
        public AimingComponent Aiming => _aim;
        private CostumesControllerComponent _costume;

        #region managedctrl

        public override void StartController()
        {

            base.StartController();

            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            _playerInputReader.EnablePlayerInputs();


            _aim = GetComponent<AimingComponent>();
            _aim.StartController();
            _costume = GetComponent<CostumesControllerComponent>();
            transform.LookAt(transform.forward);            
        }
        public override void ControllerUpdate(float delta)
        {
            if (_aim == null) return;
            base.ControllerUpdate(delta);
            //DoAiming(delta);
            _aim.ControllerUpdate(delta);

        }
        public override void StopController()
        {
            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            // not init for non game levels
            _aim.StopController();
        }

        #endregion



        #region aiming

        [Header("Aiming settings")]
        [SerializeField,Tooltip("If value is less, play rotation animation and rotate player")]
        float _minAngleToPlayRotation = 0.9f;
        public float RotationTreschold => _minAngleToPlayRotation;


        #endregion

        #region inputs section

        protected override void OnLockInputs(bool isLock)
        {
        }

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
            // nyi
        }

        private void OnPauseButton()
        {
            Debug.Log($"Pause button NYI");
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
           // AdjustInputsVector(_playerInputReader.InputDirection);
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

    }
}