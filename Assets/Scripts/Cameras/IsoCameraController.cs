using System.Linq;
using UnityEngine;

public class IsoCameraController : MonoBehaviour
{

    private Transform _target;

    private Vector3 _offset;
    private Vector3 _desiredPos;

    private void Start()
    {
        _offset = transform.position;
        if (_target == null) _target = FindObjectsOfType<Transform>().First(t => t.name == Constants.Objects.c_isoCameraTargetObjectName);
    }

    private void LateUpdate()
    {
        _desiredPos = _target.transform.forward + _target.transform.position;
        transform.position = Vector3.Slerp(transform.position, _desiredPos + _offset, Time.deltaTime);
    }

}

