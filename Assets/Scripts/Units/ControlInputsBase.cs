using Arcatech.Effects;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Skills;
using Arcatech.Stats;
using Arcatech.Triggers;
using Arcatech.Units.Inputs;
using Arcatech.Units.Stats;
using KBCore.Refs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{
    [RequireComponent(typeof(GroundDetectorPlatformCollider))]
    public abstract class ControlInputsBase : MonoBehaviour, IManagedController
    {
        [SerializeField] public bool DebugMessage = false;

        public ControlledUnit Unit { get; set; }
        [SerializeField, Self] protected Rigidbody _rb;
        protected Dictionary<MovementStatType, StatValueContainer> _stats { get; set; }

        private bool _inputsLocked;
        [SerializeField] public bool LockInputs
        {
            get
            {
                return _inputsLocked;
            }
            set
            {
                OnLockInputs(value);
                _inputsLocked = value;

                ZeroAnimatorFloats();
                //AnimateMovement();
            }
        }// todo ?
        protected abstract void OnLockInputs(bool isLock);

        protected float lastDelta;


        public ControlInputsBase PopulateDictionary()
        {
            _stats = new Dictionary<MovementStatType, StatValueContainer>();
            var vals = Enum.GetValues(typeof(MovementStatType));
            foreach (var typ in vals)
            {
                _stats[(MovementStatType)typ] = null;
            }
            return this;
        }
        public ControlInputsBase SetMovementStats(MovementStatsConfig cfg)
        {
            foreach (var stat in cfg.Stats.Keys)
            {
                _stats[stat] = new StatValueContainer(cfg.Stats[stat]);
            }
            return this;
        }
        public float GetMovementStatValue(MovementStatType t) => _stats[t].GetCurrent;

        #region ManagedController


        public virtual void ControllerUpdate(float delta)
        {
            if (LockInputs) return;
            lastDelta = delta;
            //_animator.SetFloat("AirTime", _groundedPlatform.AirTime);
        }
        public virtual void FixedControllerUpdate(float fixedDelta)
        {
            if (LockInputs) return;
            //if (_groundedPlatform.IsGrounded)
            //{
            //    UpdateAnimatorVector(DoHorizontalMovement(fixedDelta));
            //    AnimateMovement();
            //}
        }

        public virtual void StartController()
        {
            ControllerBindings(true);
        }
        public virtual void StopController()
        {
            ControllerBindings(false);
        }


        #endregion
        protected abstract ControlInputsBase ControllerBindings(bool start);


        #region movement and jumping

        [SerializeField, Self] protected GroundDetectorPlatformCollider _groundedPlatform;

        protected void DoJump()
        {
            _rb.velocity = transform.forward + transform.up;
            _rb.AddForce((transform.forward + transform.up) * _stats[MovementStatType.JumpForce].GetCurrent, ForceMode.Impulse);
            _groundedPlatform.OffTheGroundEvent += HandleLanding;
        }

        private void HandleLanding(bool startjump)
        {
            if (!startjump)
            {
                _groundedPlatform.OffTheGroundEvent -= HandleLanding;
                _animator.SetTrigger("JumpEnd");
            }
        }
        #endregion

        #region animations
        [SerializeField, Self] protected Animator _animator;
        public bool IsInMeleeCombo = false;
        Vector2 animVect;

        protected virtual void ZeroAnimatorFloats()
        {
            _animator.SetBool("Moving", false);
            _animator.SetFloat("ForwardMove", 0f);
            _animator.SetFloat("SideMove", 0f);
            _animator.SetFloat("Rotation", 0);
        }
        protected virtual void AnimateCombatActivity(UnitActionType type)
        {
            if (GameManager.Instance.GetCurrentLevelData.LevelType != LevelType.Game) return;
            switch (type)
            {
                case UnitActionType.Melee:
                    _animator.SetTrigger("MeleeAttack");
                    break;
                case UnitActionType.Ranged:
                    _animator.SetTrigger("RangedAttack");
                    break;
                case UnitActionType.DodgeSkill:
                    _animator.SetTrigger("Dodge");
                    break;
                case UnitActionType.MeleeSkill:
                    _animator.SetTrigger("MeleeSpecial");
                    break;
                case UnitActionType.RangedSkill:
                    _animator.SetTrigger("RangedSpecial");
                    break;
                case UnitActionType.ShieldSkill:
                    _animator.SetTrigger("ShieldSpecial");
                    break;
                case UnitActionType.Jump:
                    _animator.SetTrigger("JumpStart");
                    break;
            }
        }
        protected virtual void AnimateStagger()
        {
            _animator.SetTrigger("TakeDamage");
        }

        #endregion
        public void ToggleBusyControls_AnimationEvent(int state)
        {
            if (DebugMessage)
            {
                Debug.Log($"Animation event busy controls! {state}");
            }
            LockInputs = state != 0;
        }


        public event Action<UnitActionType> UnitActionRequestedEvent = delegate { };

        protected virtual void RequestCombatAction(UnitActionType type)
        {
            if (LockInputs || !_groundedPlatform.IsGrounded) return;

            if (DebugMessage)
            {
                Debug.Log($"Do combat action {type}");
            }

            switch (type)
            {
                case UnitActionType.Jump:
                    DoJump();
                    break;
                default:
                    UnitActionRequestedEvent.Invoke(type);
                    break;
            }
        }

    }
}