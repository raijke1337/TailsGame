using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{
    public class GroundDetectorPlatformCollider : MonoBehaviour
    {
        public event UnityAction<bool> OffTheGroundEvent = delegate {};

        [SerializeField] float _groundDist = 0.1f;
        [SerializeField] LayerMask _groundMask;

        public bool IsGrounded { get; private set; } = true;
        public float AirTime
        { get;
            private set;
        }

        void FixedUpdate()
        {
            bool state = Physics.CheckSphere(transform.position, _groundDist, _groundMask); // false means not standing
            if (state != IsGrounded)
            {
                OffTheGroundEvent.Invoke(!state);
                IsGrounded = state;
            }
            if (!state)
            {
                AirTime += Time.fixedDeltaTime;
            }
            if (state)
            {
                AirTime = 0f;
            }

        }

        //platforming

        Transform platform;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                // land on top
                if (collision.GetContact(0).normal.y > 0.5f)
                {
                    platform = collision.transform;
                    transform.SetParent(platform);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                transform.SetParent(null);
                platform = null;
            }
        }
    }
}