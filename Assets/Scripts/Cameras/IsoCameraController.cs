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
    [SerializeField]
    private float _camMoveSpeed = 1.4f;


    private void Start()
    {
        _target = transform.GetComponentInParent<PlayerUnit>().transform;
        transform.parent = null;
    }

    private void LateUpdate()
    {
        desired = _target.transform.forward + _target.transform.position;
        transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * _camMoveSpeed);
    }

    Vector3 desired;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(desired, 0.1f);
    }

}

