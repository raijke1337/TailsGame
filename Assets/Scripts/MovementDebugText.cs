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

public class MovementDebugText : MonoBehaviour
{
    private Text text;


    private void OnEnable()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        var _unit = GameManager._self.GetPlayerUnit;
        if (_unit == null) return;
        text.text = $"X Y Z " +
            $"\nFacing {_unit.GetDebugData._facing} " +
            $"\nMoving {_unit.GetDebugData._movement} " +
            $"\nAnimVector{_unit.GetDebugData._animVector}";
    }
}

