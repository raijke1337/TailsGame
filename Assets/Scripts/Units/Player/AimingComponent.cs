using Arcatech.EventBus;
using Arcatech.Managers;
using Arcatech.UI;
using Cinemachine.Utility;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

namespace Arcatech.Units.Inputs
{
    public class AimingComponent : MonoBehaviour, IManagedController
    {
        #region setup
        private Camera _camera;
        private Plane _aimPlane;

        [SerializeField, Range(0.1f, 3f)] float targetingSphereRadius = 1f;

        #endregion

        #region public properties
        Vector3 _target;
        float distanceToTarget;
        float _dotProduct;
        float _rotationToTarget;
        RaycastHit hit;
        public float GetDotProduct
        { get { return _dotProduct; } }

        public Vector3 GetLookTarget
        {
            get { return _target; }
        }
        public float GetDistanceToTarget
        {
            get
            {
                return distanceToTarget;
            }
        }
        /// <summary>
        /// positive = clockwise, negatve = CCW
        /// </summary>
        public float GetRotationToTarget
        {
            get => _rotationToTarget;
        }
        public Vector3 GetNormalizedDirectionToTaget
        {
            get
            {
                var heading = _target - transform.position;
                return heading.normalized;
            }
        }
        public Vector3 GetDirectionToTarget
        {
            get
            {
                return _target - transform.position;
            }
        }
        public RaycastHit GetCurrentRaycastHit { get { return hit; } }
        #endregion


        private float prevY;
        float planeY = 0f;

        #region managed
        public void StartController()
        {
            _aimPlane = new Plane(Vector3.down, planeY);

            _target = transform.forward;
            _camera = Camera.main;
        }
        public void FixedControllerUpdate(float fixedDelta)
        {

        }

        public void ControllerUpdate(float delta)
        {
            // update aim plane position
            if (transform.position.y != prevY)
            {
                _aimPlane.Translate(Vector2.down * (transform.position.y - prevY));
                prevY = transform.position.y;
            }

            // aim at plane
            ////raycast at plane
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            _aimPlane.Raycast(r, out float rayDist);
            _target = r.GetPoint(rayDist);
            Debug.Log($"{_target}");
            CheckTargetables(_target);


            var vectorToTarget = _target - transform.position;
            // new
            vectorToTarget.ProjectOntoPlane(_aimPlane.normal);

            distanceToTarget = vectorToTarget.magnitude;
            _dotProduct = Vector3.Dot(transform.forward, GetNormalizedDirectionToTaget);
            _rotationToTarget = Vector3.Cross(transform.forward, GetNormalizedDirectionToTaget).y;
        }

        ITargetable currentTgt;
        Collider[] checkColliders = new Collider[10];
        void CheckTargetables(Vector3 target)
        {
            if (target == null) return; 
            if (Physics.OverlapSphereNonAlloc(target, targetingSphereRadius, checkColliders, LayerMask.NameToLayer("Ground")) > 0) // dont check ground layer objects
            {
                if (currentTgt != null) return;

                for (int i = 0; i < checkColliders.Length; i++)
                {
                    if (checkColliders[i] == null) break;
                    if (checkColliders[i].TryGetComponent<ITargetable>(out var component))
                    {
                        currentTgt = component;
                        EventBus<PlayerTargetUpdateEvent>.Raise(new PlayerTargetUpdateEvent(component, true));
                    }
                }
            }
            else

                EventBus<PlayerTargetUpdateEvent>.Raise(new PlayerTargetUpdateEvent(currentTgt, false));
            currentTgt = null;
        }

        public void StopController()
        {

        }
        #endregion

        private void OnDrawGizmos()
        {
            if (_target != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_target, targetingSphereRadius);
            }
        }
    }
}