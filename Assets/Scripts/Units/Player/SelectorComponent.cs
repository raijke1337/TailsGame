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

public class SelectorComponent : MonoBehaviour
{

    private Ray ray;
    public Vector3? CalculateAimPoint()
    {
        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit)) return null;

        var loc = hit.point;
        return new Vector3(loc.x, 0, loc.z);
    }
}

