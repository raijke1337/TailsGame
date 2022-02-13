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
using Zenject;

public class MovementDebugText : MonoBehaviour
{
    private Text text;
    [Inject]
    private PlayerUnit _player;

    private void OnEnable()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        if (_player == null) return;
        text.text = $"X Y Z " +
            $"\nFacing {_player.GetDebugData._facing} " +
            $"\nMoving {_player.GetDebugData._movement} " +
            $"\nAnimVector{_player.GetDebugData._animVector}";
    }
}

