using ECM.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Units
{
    public class MovementControllerComponent : BaseCharacterController
    {
        Rigidbody _rb;
        public Vector3 LookDirection { get; set; }
        public float AirTime { get; protected set; } = 0f;
        public void SetMoveDirection(Vector3 dir) => moveDirection = dir;
        public void DoJump()
        {
            StartCoroutine(JumpToggle());
        }
        protected IEnumerator JumpToggle()
        {
            jump = true;
            yield return null;
            jump = false;
        }



        protected override void Animate()
        {
            var fwd = transform.forward;
            var right = transform.right;

            // Dot product of two vectors determines how much they are pointing in the same direction.
            // If the vectors are normalized (transform.forward and right are)
            // then the value will be between -1 and +1.
            var x = Vector3.Dot(right, Vector3.Normalize(_rb.velocity));
            var z = Vector3.Dot(fwd, Vector3.Normalize(_rb.velocity));

            animator.SetFloat("ForwardMove", z);
            animator.SetFloat("SideMove", x);

            animator.SetFloat("VerticalMove", _rb.velocity.y);
            animator.SetFloat("Rotation", Vector3.Cross(fwd,LookDirection).y);
            
            animator.SetBool("isMoving",(_rb.velocity.sqrMagnitude!= 0));


            if (!movement.isGrounded)
            {
                AirTime += Time.fixedDeltaTime;
                animator.SetFloat("AirTime", AirTime);
            }
            if (movement.isGrounded)
            {
                if (!movement.wasGrounded)
                {
                    animator.SetTrigger("Land");
                }
                AirTime = 0f;
                animator.SetFloat("AirTime", AirTime);
            }

        }
        protected override void HandleInput()
        {

        }
        // call separately
        protected override void UpdateRotation()
        {
            RotateTowards(LookDirection);
        }
        public override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody>();
        }

    }
}