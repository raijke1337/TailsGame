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
            _offset = transform.position; 

            if (_target == null) _target = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit.transform;
        }

        public override void RunUpdate(float delta)
        {
            _desiredPos = _target.transform.forward + _target.transform.position;
            transform.position = Vector3.Slerp(transform.position, _desiredPos + _offset, Time.deltaTime*2);
        }

        public override void Stop()
        {

        }
    }

}