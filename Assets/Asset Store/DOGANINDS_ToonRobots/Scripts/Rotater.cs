using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGN_Robots
{
    public class Rotater : MonoBehaviour
    {
        public float rotationSpeed = 40f;

        void Update()
        {
            transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
        }
    }

    
}
