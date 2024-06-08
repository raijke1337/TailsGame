using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Units
{
    [RequireComponent(typeof(BoxCollider))]
    public class GroundDetectorPlatformCollider : MonoBehaviour
    {
        private BoxCollider _col;

        public event SimpleEventsHandler<string> PlatfromCollidedWithTagEvent;

        private void Start()
        {
            _col = GetComponent<BoxCollider>();
            //_col.isTrigger = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlatfromCollidedWithTagEvent?.Invoke(other.tag);
        }



    }
}