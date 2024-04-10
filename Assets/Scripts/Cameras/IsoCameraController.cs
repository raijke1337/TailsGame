using UnityEngine;
namespace Arcatech.Managers
{
    [RequireComponent(typeof(Camera))]
    public class IsoCameraController : LoadedManagerBase
    {

        [SerializeField] private Transform _target;

        private Vector3 _offset;
        private Vector3 _desiredPos;
        private Camera _camera;
        private MeshRenderer _renderer;

        public override void Initiate()
        {
            _camera = GetComponent<Camera>();
            _offset = transform.position; // todo? maybe create a separate setting for this

            if (_target == null) _target = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit.transform;
        }

        public override void RunUpdate(float delta)
        {
            _desiredPos = _target.transform.forward + _target.transform.position;
            transform.position = Vector3.Slerp(transform.position, _desiredPos + _offset, Time.deltaTime);
            //Ray ray = new Ray(_camera.transform.position, transform.forward);
            //if (Physics.Raycast(ray, out var hit))
            //{
            //    if (hit.collider.CompareTag("StaticItem"))
            //    {
            //        var r = hit.collider.GetComponent<MeshRenderer>();
            //        if (r != _renderer)
            //        {
            //            if (_renderer != null)
            //            {
            //                _renderer.enabled = true;
            //            }
            //            _renderer = r;
            //            _renderer.enabled = false;

            //            Debug.Log($"Hiding item {hit.collider.name}");
            //        }
            //    }
            //    else
            //    {
            //        if (_renderer != null)
            //        {
            //            Debug.Log($"Showing item {_renderer.name}");
            //            _renderer.enabled = true;
            //            _renderer = null;
            //        }
            //    }
            //}
        }

        public override void Stop()
        {

        }
    }

}