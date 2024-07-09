using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{
    public class GroundDetectorPlatformCollider : MonoBehaviour
    {
        #region platfroming
        Transform platform;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                platform = collision.transform;
                transform.SetParent(platform);
                // land on top
                //if (collision.GetContact(0).normal.y > 0.5f)
                //{

                //}
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!collision.gameObject.CompareTag("MovingPlatform"))
            {
                transform.SetParent(null);
                platform = null;
            }
            else
            {
                if (collision.gameObject != platform.gameObject)
                {
                    platform = collision.transform;
                    transform.SetParent(platform);
                }
            }
        }

        #endregion

        [SerializeField] float _groundDist = 0.1f;
        [SerializeField] LayerMask _groundMask;

        public event UnityAction<bool> OffTheGroundEvent = delegate {};

        public bool IsGrounded { get;private set; }
        public float AirTime
        { get;
            private set;
        }

        void FixedUpdate()
        {
            bool state = Physics.CheckSphere(transform.position, _groundDist, _groundMask);

            if (!state)
            { 
                AirTime += Time.fixedDeltaTime;
                if (IsGrounded)
                {
                    OffTheGroundEvent.Invoke(true);
                } 
            }
            if (state)
            {
                if (!IsGrounded)
                {
                    OffTheGroundEvent.Invoke(false);
                }
                AirTime = 0f;
            }
            IsGrounded = state;
        }


    }
}