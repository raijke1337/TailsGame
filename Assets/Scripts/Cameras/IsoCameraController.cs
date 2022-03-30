using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class IsoCameraController : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private float _camMoveSpeed = 1.4f;

    private Vector3 _offset;
    private Vector3 _desiredPos;

    private void Start()
    {
        _offset = transform.position;
    }

    private void LateUpdate()
    {
        _desiredPos = _target.transform.forward + _target.transform.position;
        transform.position = Vector3.Slerp(transform.position, _desiredPos + _offset, Time.deltaTime);
    }

}

