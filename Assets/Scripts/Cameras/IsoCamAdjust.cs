using UnityEngine;

namespace Arcatech.Scenes.Cameras
{
    public class IsoCamAdjust
    {
        public Vector3 Isoforward;
        public Vector3 Isoright;

        private void AdjustDirections()
        {
            Isoforward = Camera.main.transform.forward;
            Isoforward.y = 0;
            Isoforward = Vector3.Normalize(Isoforward);
            Isoright = Quaternion.Euler(new Vector3(0, 90, 0)) * Isoforward;
        }

        public IsoCamAdjust()
        {
            AdjustDirections();
        }
    }



}