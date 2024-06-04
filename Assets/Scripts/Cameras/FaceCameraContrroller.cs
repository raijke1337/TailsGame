using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Scenes.Cameras
{
    public class FaceCameraCOntrroller : MonoBehaviour
    {
        public FaceCameraTargetComponent Target;
        public Vector3 DesiredOffset;

        private void OnEnable()
        {
            Assert.IsNotNull(Target);
            transform.position = Target.transform.position + DesiredOffset;
            transform.LookAt(Target.transform.position);
        }
        private void Update()
        {
            
        }
    }
}