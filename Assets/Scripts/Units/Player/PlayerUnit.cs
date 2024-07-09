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
        [SerializeField] float _smoothTime = 0.2f;



        protected const float zeroF = 0f;

        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            _adj ??= new IsoCamAdjust();
            _playerInp = _inputs as InputsPlayer;
            ToggleCamera(true);
        }
        protected override void SetupStateMachine()
        {
            UnitStateMachine = new ArcaStateMachine();
            var locom = new UnarmedLocomotionState (this, _animator);
            var idle = new UnarmedIdleState(this, _animator);

            UnitStateMachine.AddAnyTransition(idle, new FuncPredicate(IdleConditions));

            UnitStateMachine.AddTransition(idle, locom, new FuncPredicate();

            UnitStateMachine.ForceState(idle);



        }

        #region predicates
        protected bool _unitMoving = false;
        protected override bool IdleConditions()
        {
            return animationVector.sqrMagnitude == zeroF;
        }

        protected override bool UnitInAir()
        {
            throw new System.NotImplementedException();
        }

        #endregion




        #region movement

        Vector2 inputsVector;

        Vector3 animationVector = Vector3.zero;
        Vector3 _dampingVelocity;
        Vector3 _dampingRotation;

        public override void DoHorizontalMovement(float delta)
        {
            // movement
            Vector3 AD = _adj.Isoright * _playerInp.InputVector.x;
            Vector3 WS = _adj.Isoforward * _playerInp.InputVector.y;
            var _rotatedInputsVector = AD + WS;

            if (_rotatedInputsVector.magnitude > zeroF)
            {
                var velocity = _rotatedInputsVector * (_inputs.GetMovementStatValue(MovementStatType.MoveSpeed) * delta);
                _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);

                UpdateAnimatorVector(velocity);

                DampAnimationVector(animationVector);
            }
            else
            {
                var velocity = _rotatedInputsVector * (_inputs.GetMovementStatValue(MovementStatType.MoveSpeed) * delta);
                _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);

                DampAnimationVector(Vector3.zero);
                _rb.velocity = new Vector3(zeroF, _rb.velocity.y, zeroF);

                UpdateAnimatorVector(velocity);            
            }
            //animation 
            AnimateMovement();
        }
        void UpdateAnimatorVector(Vector3 movement)
        {
            var playerFwd = transform.forward;
            var playerRght = transform.right;
            movement.Normalize();
            // Dot product of two vectors determines how much they are pointing in the same direction.
            // If the vectors are normalized (transform.forward and right are)
            // then the value will be between -1 and +1.
            var x = Vector3.Dot(playerRght, movement);
            var z = Vector3.Dot(playerFwd, movement);
            animationVector.x = x;
            animationVector.y = movement.y; // vertical for fall animations
            animationVector.z = z;
        }
        private void DampAnimationVector(Vector3 target)
        {
            // if in air, return TODO
            animationVector = Vector3.SmoothDamp(animationVector, target, ref _dampingVelocity, _smoothTime);
        }

        void AnimateMovement()
        {
            _animator.SetFloat("VerticalMove", _rb.velocity.y);

            _animator.SetFloat("ForwardMove", animationVector.z);
            _animator.SetFloat("SideMove", animationVector.x);
        }
        public override void DoRotation(float d)
        {
            if (inputsVector.magnitude == zeroF && _playerInp.Aiming.GetDotProduct < _playerInp.RotationTreschold)
            {
                _animator.SetFloat("Rotation", _playerInp.Aiming.GetRotationToTarget);
                transform.LookAt(Vector3.SmoothDamp(transform.position, _playerInp.Aiming.GetLookTarget, ref _dampingRotation, _smoothTime));
            }
            else
            {
                transform.LookAt(Vector3.SmoothDamp(transform.position, _playerInp.Aiming.GetLookTarget, ref _dampingRotation, _smoothTime));
                _animator.SetFloat("Rotation", 0);
            }
        }
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




        //public override void AddItem(ItemSO item, bool equip)
        //{
        //    base.AddItem(item,equip);
        //    _animator.SetTrigger("ItemPickup");
        //    if (_inputs.DebugMessage)
        //    {
        //        Debug.Log($"Player picked up item {item}");
        //    }
        //}
    }
    #endregion
    #endregion

}