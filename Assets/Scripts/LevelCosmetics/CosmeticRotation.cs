using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Level
{
    public class CosmeticRotation : MonoBehaviour
    {
        [SerializeField] protected float _rotationSpeed;
        //public bool RotateX;
        //public bool RotateY; < - this
        //public bool RotateZ;

        private void Update()
        {
            transform.Rotate(Vector3.up, _rotationSpeed);
        }
    }
}