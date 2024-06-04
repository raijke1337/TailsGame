using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Scenes.Cameras
{
    public class FaceCameraTargetComponent : MonoBehaviour
    {
        public Transform TargetBone;
        [Range(0.1f,3f)]public float LerpSpeed;

        private void OnEnable()
        {
            Assert.IsNotNull(TargetBone);
            transform.SetLocalPositionAndRotation(transform.position, transform.rotation);
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, TargetBone.position, LerpSpeed*Time.deltaTime);
        }
    }
}