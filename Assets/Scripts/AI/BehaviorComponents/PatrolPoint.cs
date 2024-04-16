
using UnityEngine;
namespace Arcatech.AI
{
    public class PatrolPoint : MonoBehaviour, ITargetPoint
    {
        public Color GizmoColor;

        public float AssessDistanceTo(Vector3 position)
        {
            return Vector3.Distance(position, transform.position);
        }

        private void OnValidate()
        {

        }
        private void Awake()
        {
            
        }
    }
}