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
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : UnitController
{
    private PlayerControls _controls;
    public Vector3 CurrentCursorPosition {get;private set;}


    private void OnEnable()
    {
        _controls.Movement.Enable();
        _controls._2h.Enable();
    }
    private void OnDisable()
    {
        _controls.Movement.Disable();
        _controls._2h.Disable();
    }

    protected override void Awake()
    {
        _controls = new PlayerControls();
        _controls._2h.Attack.performed += OnAttack2h;
        _controls.Movement.Q.performed += OnSpecialQ;
        _controls.Movement.PauseEditor.performed += OnPauseEditor;
        base.Awake();
    }

    private void OnAttack2h(CallbackContext context)
    {
        CallOnMeleeAttack();
    }
    private void OnSpecialQ (CallbackContext context)
    {
        CallOnQSpecial();
    }
#if UNITY_EDITOR
    private void OnPauseEditor(CallbackContext context)
    {
        EditorApplication.isPaused = true;
    }
#endif


    private void Update()
    {
        Aim();
        CalculateMovement();
    }
    private void CalculateMovement()
    {
        var input = _controls.Movement.WASD.ReadValue<Vector2>();
        Vector3 AD = Isoright * input.x;
        Vector3 WS = Isoforward * input.y;
        _movement = AD + WS;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (GetRaycastPointAndFocusable() == null) return;
        Gizmos.DrawSphere((Vector3)GetRaycastPointAndFocusable(), 0.1f);

        // draws the forward vector of unit
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 300);
    }
#endif
    private void Aim()
    {
        //todo in progress

        var point = GetRaycastPointAndFocusable();
        if (point == null) return;
        else
        {
            CurrentCursorPosition = (Vector3)point;
            transform.LookAt(CurrentCursorPosition);
        }
    }
    private Vector3? GetRaycastPointAndFocusable()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit))
        {
            return (hit.point - new Vector3(0, hit.point.y));
        }
        else
        {
            return null;
        }
    }


}

