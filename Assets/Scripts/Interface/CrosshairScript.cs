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

public class CrosshairScript : MonoBehaviour
{
    [SerializeField] private Sprite[] Shapes;
    [SerializeField] private float _aimOffset = 0.1f;

    private Vector3 _adjustedTgt;

    public void SetLookTarget(Vector3 target)
    {
        _adjustedTgt = new Vector3(target.x, _aimOffset, target.z);
    }

    private void Update()
    {
        transform.position = _adjustedTgt;
    }

}

