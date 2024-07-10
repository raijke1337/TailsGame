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
        public override ReferenceUnitType GetUnitType() => ReferenceUnitType.Player;

        [SerializeField, Child] protected Camera _faceCam;
        public AimingComponent GetAimingComponent => (_inputs as InputsPlayer).Aiming;
        [Header("Aiming settings")]
        [SerializeField, Tooltip("If value is less, play rotation animation and rotate player")]
        float _minAngleToPlayRotation = 0.9f;

        protected void ToggleCamera(bool value) { _faceCam.enabled = value; }

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            ToggleCamera(true);
        }
        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            
            var playerFwd = transform.forward;
            var playerRght = transform.right;

            // Dot product of two vectors determines how much they are pointing in the same direction.
            // If the vectors are normalized (transform.forward and right are)
            // then the value will be between -1 and +1.
            var x = Vector3.Dot(playerRght, Vector3.Normalize(_inputs.InputsMovementVector));
            var z = Vector3.Dot(playerFwd, Vector3.Normalize(_inputs.InputsMovementVector));
            Vector3 newAnim = new Vector3(x, 0, z);
            var animationVector = newAnim;

            _animator.SetFloat("VerticalMove", _rb.velocity.y);
            _animator.SetFloat("ForwardMove", animationVector.z);
            _animator.SetFloat("SideMove", animationVector.x);
            _animator.SetFloat("Rotation", GetAimingComponent.GetRotationToTarget);
        }

        #region state machine
        protected override void SetupStateMachine()
        {
            UnitStateMachine = new ArcaStateMachine();

            var locom = new UnarmedLocomotionState(movement,this, _animator);
            var idle = new UnarmedIdleState(movement, this, _animator);
            var rotation = new StandingRotationState(movement, this, _animator);
            var inAir = new InAirState(movement, this, _animator);


            
            UnitStateMachine.AddAnyTransition(inAir, new FuncPredicate(InAirCondition));
            UnitStateMachine.AddAnyTransition(idle, new FuncPredicate(IdleConditions));
            UnitStateMachine.AddAnyTransition(locom, new FuncPredicate(MovingCondition));

            UnitStateMachine.AddTransition(idle, rotation, new FuncPredicate(StandingRotationCondition));

            UnitStateMachine.ForceState(idle);
        }

        #region predicates
        bool InAirCondition()
        {
            return !movement.isOnGround;
        }
        bool JumpCondition()
        {
            return jumpPressed;
        }
        bool MovingCondition()
        {
            return movement.isOnGround && _inputs.InputsMovementVector.magnitude > 0;
        }
        bool IdleConditions()
        {
            return movement.isOnGround && _inputs.InputsMovementVector.magnitude == 0;
        }
        bool StandingRotationCondition()
        {
            return movement.isOnGround && GetAimingComponent.GetDotProduct < _minAngleToPlayRotation;
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


        

    }

}