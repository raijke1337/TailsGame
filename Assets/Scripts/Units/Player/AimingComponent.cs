using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;
namespace Arcatech.Units.Inputs
{
    public class AimingComponent : ManagedControllerBase
    {
        private Camera _camera;
        public SimpleEventsHandler<bool> SelectionUpdatedEvent;
        private float _tgtDelay;

        [Tooltip("Vertical offset for raycast plane"), SerializeField] private float _vertOffset = 0.1f;
        public Vector3 GetLookPoint => _mousePos;
        public SelectableItem GetSelectableItem
        {
            get => _item;
            private set
            {
                _item = value;
                SelectionUpdatedEvent?.Invoke(value != null);
            }
        }
        private SelectableItem _item;
        private Plane _plane;
        private Vector3 _mousePos;

        private void SetMousePos(Ray r)
        {
            // aiming

            if (_plane.Raycast(r, out float rayDist))
            {
                _mousePos = r.GetPoint(rayDist);
            }
            //
        }
        private void SetSelectable(Ray r)
        {
            if (_tgtDelay >= 0.5f)
            {
                _tgtDelay = 0;
                //selectable item
                if (Physics.Raycast(r, out RaycastHit hitInfo)) // todo dunno if good performance maybe do a coroutine instead
                {
                    var i = hitInfo.collider.gameObject.GetComponent<SelectableItem>();
                    GetSelectableItem = i;
                }
            }
        }





        #region managed
        public override void StartController()
        {
            _plane = new Plane(Vector3.down, _vertOffset);
            _mousePos = transform.forward;
            _camera = Camera.main;
        }

        public override void UpdateController(float delta)
        {
            Ray r = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            SetMousePos(r);
            _tgtDelay += delta;
            //SetSelectable(r);
        }

        public override void StopController()
        {

        }

        #endregion
    }

}