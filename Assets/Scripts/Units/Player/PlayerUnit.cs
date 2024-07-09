using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Scenes.Cameras;
using Arcatech.StateMachine;
using Arcatech.Texts;
using Arcatech.Units.Inputs;
using KBCore.Refs;
using UnityEngine;


namespace Arcatech.Units
{
    [RequireComponent(typeof(InputsPlayer))]
    public class PlayerUnit : ControlledUnit
    {
        [SerializeField, Child] protected Camera _faceCam;
        public AimingComponent GetAimingComponent => (_inputs as InputsPlayer).Aiming;
        private InputsPlayer _playerInp;
        private IsoCamAdjust _adj;

        #region player movement
        [Header("Player movement settings")]
        [SerializeField] float _smoothTime = 0.5f;
        protected const float zeroF = 0f;
        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            _adj ??= new IsoCamAdjust();
            _playerInp = _inputs as InputsPlayer;
            ToggleCamera(true);
        }
        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);

                Vector3 AD = _adj.Isoright * _playerInp.InputVector.x;
                Vector3 WS = _adj.Isoforward * _playerInp.InputVector.y;

            movementVector = AD + WS;

            var playerFwd = transform.forward;
            var playerRght = transform.right;

            // Dot product of two vectors determines how much they are pointing in the same direction.
            // If the vectors are normalized (transform.forward and right are)
            // then the value will be between -1 and +1.
            var x = Vector3.Dot(playerRght, Vector3.Normalize(movementVector));
            var z = Vector3.Dot(playerFwd, Vector3.Normalize(movementVector));
            Vector3 newAnim = new Vector3(x, 0, z);
            animationVector = newAnim;

            _animator.SetFloat("VerticalMove", _rb.velocity.y);
            _animator.SetFloat("ForwardMove", animationVector.z);
            _animator.SetFloat("SideMove", animationVector.x);
            _animator.SetFloat("Rotation", _playerInp.Aiming.GetRotationToTarget);
        }
        Vector3 SmoothDampVector(in Vector3 current, Vector3 target)
        {
           // Debug.Log($"Damp from {current} to {target}");
            return Vector3.SmoothDamp(current, target, ref movementDampingVelocity, _smoothTime);
        }


        #region state machine
        protected override void SetupStateMachine()
        {
            UnitStateMachine = new ArcaStateMachine();

            var locom = new UnarmedLocomotionState(this, _animator);
            var idle = new UnarmedIdleState(this, _animator);
            var rotation = new StandingRotationState(this, _animator, 0.2f);
            var inAir = new InAirState(_groundedPlatform, this, _animator);

            UnitStateMachine.AddAnyTransition(inAir, new FuncPredicate(InAirCondition));
            UnitStateMachine.AddAnyTransition(idle, new FuncPredicate(IdleConditions));
            UnitStateMachine.AddAnyTransition(locom, new FuncPredicate(MovingCondition));


            UnitStateMachine.AddTransition(idle, rotation, new FuncPredicate(RotationCondition));

            UnitStateMachine.ForceState(idle);
        }

        #region predicates
        bool InAirCondition()
        {
            return !_groundedPlatform.IsGrounded;
        }
        protected override bool IdleConditions()
        {
            return UnitStateMachine.StateExpired && _groundedPlatform.IsGrounded && movementVector.magnitude < 0.1f && _playerInp.Aiming.GetDotProduct >= _playerInp.RotationTreschold;
        }
        bool RotationCondition()
        {
            return UnitStateMachine.StateExpired && _groundedPlatform.IsGrounded && _playerInp.InputVector.magnitude == zeroF && _playerInp.Aiming.GetDotProduct < _playerInp.RotationTreschold;
        }
        bool MovingCondition()
        {
            return UnitStateMachine.StateExpired && _groundedPlatform.IsGrounded && movementVector.magnitude > 0;
        }

        #endregion

        #region movement
        Vector3 movementVector;
        Vector3 movementDampingVelocity;
        Vector3 animationVector;
        public override void DoHorizontalMovement(float delta)
        {
            var velocity = movementVector * (_inputs.GetMovementStatValue(MovementStatType.MoveSpeed) * delta);
            _rb.velocity = velocity;
        }
        public override void DoRotation(float d)
        {
            transform.LookAt(_playerInp.Aiming.GetLookTarget);
        }
        #endregion
        #region jump

        protected override void DoJump()
        {
            if (_groundedPlatform.IsGrounded)
            {
                _rb.velocity = transform.forward + transform.up;
                _rb.AddForce((transform.forward + transform.up) * movementStats.Stats[MovementStatType.JumpForce].Start, ForceMode.Impulse);
            }
        }
        #endregion

        #endregion
        #region inventory
        public bool PlayerArmed => _inventory.GetWeapons.Length > 0;

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
        protected override void RaiseStatChangeEvent(StatChangedEvent ev)
        {
            EventBus<StatChangedEvent>.Raise(ev);
            base.RaiseStatChangeEvent(ev);
        }
        #endregion

        #region animator

        public void ComboWindowStart()
        {
            _animator.SetBool("AdvancingCombo", true);
            // _playerController.IsInMeleeCombo = true;
        }
        public void ComboWindowEnd()
        {
            _animator.SetBool("AdvancingCombo", false);
            //_playerController.IsInMeleeCombo = false;
        }
        public void PlayerIsTalking(DialoguePart d)
        {
            if (_inputs.DebugMessage)
                Debug.Log($"Dialogue window is shown by player panel script, dialogue is: {d.Mood}");
        }

        protected override void HandleDamage(float value)
        {
            base.HandleDamage(value);

            if (_inputs.DebugMessage)
                Debug.Log($"Player unit handle dmg event");
        }

        protected override void HandleDeath()
        {
            _animator.SetTrigger("KnockDown");
            if (_inputs.DebugMessage)
                Debug.Log($"Player unit handle death event");
        }

        public override ReferenceUnitType GetUnitType()
        {
            return ReferenceUnitType.Player;
        }


    }
    #endregion
    #endregion

}