using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Units
{
    public class StickyFeetComponent : MonoBehaviour
    {
        [SerializeField] string _platformsTag = "MovingPlatform";
        Transform parent;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(_platformsTag))
            {
                // land on top
                if (collision.GetContact(0).normal.y > 0.5f)
                {
                    parent = collision.transform;
                    transform.SetParent(parent);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag(_platformsTag))
            {
                transform.SetParent(null);
                parent = null;
            }
        }
    }
}