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

namespace Arcatech.Units
{
    [RequireComponent(typeof(GroundDetectorPlatformCollider))]
    public abstract class ControlInputsBase : ManagedControllerBase
    {
        protected const float zeroF = 0f;
        public BaseUnit Unit { get; set; }
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
                AnimateMovement();
            }
        }// todo ?
        protected abstract void OnLockInputs(bool isLock);

        protected float lastDelta;

        //specific 
        public event SimpleEventsHandler StaggerHappened;
        public event SimpleEventsHandler ShieldBreakHappened;
        public event SimpleEventsHandler<float> DamageHappened;
        public event SimpleEventsHandler ZeroHealthHappened;
        public event SkillRequestedEvent SkillSpawnEvent;

        protected virtual void ShieldBreakEventCallback() => ShieldBreakHappened?.Invoke();

        #region setup


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

        #region ManagedController


        public override void UpdateController(float delta)
        {
            if (LockInputs) return;
            lastDelta = delta;

            _animator.SetFloat("AirTime", _groundedPlatform.AirTime);

            if (_groundedPlatform.IsGrounded)
            {
                UpdateAnimatorVector(DoHorizontalMovement(delta));
                AnimateMovement();
            }
        }
        public override void StartController()
        {
            ControllerBindings(true);
            base.StartController();
            _isReady = true;

        }
        public override void StopController()
        {
            ControllerBindings(false);
            _isReady = false;
        }
        #endregion
        protected abstract ControlInputsBase ControllerBindings(bool start);

        #endregion




        #region movement and jumping

        [SerializeField, Self] protected GroundDetectorPlatformCollider _groundedPlatform;
        protected abstract Vector3 DoHorizontalMovement(float delta);

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
        [SerializeField,Self] protected Animator _animator;
        public bool IsInMeleeCombo = false;
        Vector2 animVect;

        protected virtual void ZeroAnimatorFloats()
        {
            _animator.SetBool("Moving", false);
            _animator.SetFloat("ForwardMove", 0f);
            _animator.SetFloat("SideMove", 0f);
            _animator.SetFloat("Rotation", 0);
        }


        //  calculates the vector which is used to set values in animator
        protected void UpdateAnimatorVector(Vector3 movement)
        {
            var playerFwd = transform.forward;
            var playerRght = transform.right;
            movement.y = 0;
            movement.Normalize();
            // Dot product of two vectors determines how much they are pointing in the same direction.
            // If the vectors are normalized (transform.forward and right are)
            // then the value will be between -1 and +1.
            var x = Vector3.Dot(playerRght, movement);
            var z = Vector3.Dot(playerFwd, movement);
            animVect.x = x;
            animVect.y = z;
        }
        protected virtual void AnimateMovement()
        {
            if (animVect.x == 0f && animVect.y == 0f)
            {
                _animator.SetBool("Moving", false);
                _animator.SetFloat("ForwardMove", 0f);
                _animator.SetFloat("SideMove", 0f);

                // rotate in place
                if (this is InputsPlayer p)
                {
                    if (p.PlayerInputsDot < p.RotationTreschold)
                    {
                        _animator.SetFloat("Rotation", p.PlayerInputsCross);
                    }
                    else
                    {
                        _animator.SetFloat("Rotation", 0);
                    }
                }

            }
            else
            {
                _animator.SetFloat("Rotation", 0);
                _animator.SetBool("Moving", true);

                _animator.SetFloat("ForwardMove", animVect.y);
                _animator.SetFloat("SideMove", animVect.x);

            }
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


        protected virtual void RequestCombatAction(UnitActionType type)
        {
            if (LockInputs || !_groundedPlatform.IsGrounded) return;

            if (DebugMessage)
            {
                Debug.Log($"Do combat action {type}");
            }

            switch (type)
            {
                case UnitActionType.Melee:
                    break;
                case UnitActionType.Ranged:
                    break;
                case UnitActionType.DodgeSkill:
                    break;
                case UnitActionType.MeleeSkill:
                    break;
                case UnitActionType.RangedSkill:
                    break;
                case UnitActionType.ShieldSkill:
                    break;
                case UnitActionType.Jump:
                    DoJump();
                    break;
            }
        }

    }
}