using UnityEngine;
namespace Arcatech.Managers
{
    public class IsoCameraController : LoadedManagerBase
    {

        [SerializeField] private Transform _target;

        private Vector3 _offset;
        private Vector3 _desiredPos;

        public Vector3 Offset { get => _offset; set { _offset = value; } }
        // todo - can use this for scenes etc

        public override void Initiate()
        {
            _offset = transform.position; // todo? maybe create a separate setting for this

            if (_target == null) _target = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit.transform;
        }

        public override void RunUpdate(float delta)
        {
            _desiredPos = _target.transform.forward + _target.transform.position;
            transform.position = Vector3.Slerp(transform.position, _desiredPos + _offset, Time.deltaTime);
        }

        public override void Stop()
        {

        }
    }

}