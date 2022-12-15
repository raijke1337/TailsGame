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
using RotaryHeart.Lib.SerializableDictionary;

public class AimingComponent : MonoBehaviour
{
    private Camera _camera;

    [Tooltip("Vertical offset for crosshair"), SerializeField] private float _vertOffset = 0.1f;
    public Vector3 GetLookPoint { get; private set; }
    public bool IsRunning { get; private set; } = false;

    private Vector3 _prevPos = Vector3.zero;

    private void Update()
    {
        if (!IsRunning) return;
        GetLookPoint = GetCursorPosition();
    }

    private void Start()
    {
        GetLookPoint = transform.forward;
        _camera = Camera.main;
        IsRunning = true;
    }

    private Vector3 GetCursorPosition()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit)) //&& hit.collider.CompareTag("Ground")
        {
            var v = new Vector3(hit.point.x, _vertOffset, hit.point.z);
            _prevPos = v;
            return v;
        }

        else return _prevPos;
    }


}

