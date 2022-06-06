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
    public SerializableDictionaryBase<CursorType, Sprite> Cursors;
    private CrosshairComponent _crosshair;
    private Camera _camera;

    [Tooltip("The plane to cast raycasts at"),SerializeField] private Transform _raycastPrefab;
    [Tooltip("Vertical offset for crosshair"), SerializeField] private float _vertOffset = 0.1f;
    public Vector3 GetLookPoint { get; private set; }
    public InteractiveItem GetItem { get;private set; }

    public bool IsRunning { get; private set; } = false;

    private Vector3 _prevPos = Vector3.zero;

    private void Update()
    {
        if (!IsRunning) return;
        GetLookPoint = GetCursorPosition();
        _crosshair.SetScreenPosition(GetLookPoint);
        GetItem = GetItemUnderCursor();
    }

    private void Start()
    {

        GetLookPoint = transform.forward;
        _crosshair = GetComponentInChildren<CrosshairComponent>();
        _crosshair.transform.SetParent(null);
        _crosshair.SetCrosshair(Cursors[CursorType.Explore]);

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
    private InteractiveItem GetItemUnderCursor()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit)) //&& hit.collider.CompareTag("Ground")
        {
            if (hit.collider.GetComponent<NPCUnit>() != null)
            {
                _crosshair.SetCrosshair(Cursors[CursorType.EnemyTarget]);
                var unit = hit.collider.GetComponent<NPCUnit>();
                return unit;
            }
            if (hit.collider.GetComponent<LevelItemTrigger>() != null)
            {
                _crosshair.SetCrosshair(Cursors[CursorType.Item]);
                var item = hit.collider.GetComponent<LevelItemTrigger>();
                return item;
            }
            else
            {
                _crosshair.SetCrosshair(Cursors[CursorType.Explore]);
                return null;
            }
            // todo ?
        }
        else
        {
            _crosshair.SetCrosshair(Cursors[CursorType.Explore]);
            return null;
        }
    }

}

