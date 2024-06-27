using Arcatech.Managers;
using Arcatech.UI;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

namespace Arcatech.Units.Inputs
{
    public class AimingComponent : ManagedControllerBase
    {
        #region setup
        private Camera _camera;
        private Plane _aimPlane;

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


        public SimpleEventsHandler<bool, BaseTargetableItem> SelectionUpdatedEvent;


        #region managed
        public override void StartController()
        {
            _aimPlane = new Plane(Vector3.down, planeY);

            _target = transform.forward;
            _camera = Camera.main;
        }
        public override void FixedControllerUpdate(float fixedDelta)
        {

        }

        public override void ControllerUpdate(float delta)
        {

            if (transform.position.y != prevY)
            {
                _aimPlane.Translate(Vector2.down * (transform.position.y - prevY));
                prevY = transform.position.y;
            }


            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out var newhit))
            {
                if (newhit.collider != hit.collider)
                {
                    hit = newhit;
                }
                _target = hit.transform.position;
            }

            // no object, aim at plane
            if (_aimPlane.Raycast(r, out float rayDist))
            {
                _target = r.GetPoint(rayDist);
            }

            // hit a selectable
            if (Physics.Raycast(r, out hit))
            {
                if (hit.collider.gameObject.TryGetComponent<BaseTargetableItem>(out var item))
                {
                    //_aimedObject = item;
                    _target = hit.collider.transform.position; // aim at the center of the target
                    SelectionUpdatedEvent?.Invoke(true, item);
                }
            }

            var vectorToTarget = _target - transform.position;
            distanceToTarget = vectorToTarget.magnitude;
            _dotProduct = Vector3.Dot(transform.forward, GetNormalizedDirectionToTaget);
            _rotationToTarget = Vector3.Cross(transform.forward, GetNormalizedDirectionToTaget).y;


        }




        public override void StopController()
        {

        }

        #endregion



    }
}