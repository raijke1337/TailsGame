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

public class PatrolPointsGizmoDrawer : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;
    [SerializeField] private float _size = 0.3f;

    private void OnValidate()
    {
        UpdateInfo();
    }

    private void OnDrawGizmos()
    {
        UpdateInfo();
        Gizmos.color = Color.cyan;

        foreach (var p in _points)
        {
            Gizmos.DrawWireSphere(p.position, _size);
        }
    }

    [ContextMenu("Refresh")]
    public void UpdateInfo()
    {
        _points = new List<Transform>();
        _points.AddRange(GetComponentsInChildren<Transform>());
    }

}

