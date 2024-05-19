using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Units
{
    [RequireComponent(typeof(BoxCollider))]
    public class GroundDetectorPlatformCollider : MonoBehaviour
    {
        private BoxCollider _col;
        private bool _ai = false;
        public bool IsAirborne
        {
            get => _ai;
            set
            {
                _ai = value;
                //Debug.Log($"airborne: {value}");
            }
        }
                
                

        private void Start()
        {
            _col = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"Trigger enter {other}");
            if (!other.gameObject.CompareTag("Player"))
            {
                IsAirborne = false;
            }
        }
        private void OnTriggerExit(Collider other)
        {

        }


    }
}