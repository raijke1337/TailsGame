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

public class TimeController : Time
{
    private float _gameCoef = 1f;
    private float _uiCoef = 0f;

    public float GameDeltaTime => _gameCoef * deltaTime;
    public float UIdeltaTime => _uiCoef * deltaTime;

    public void SetGameDeltaTimeK(float k) => _gameCoef = k;
    public void SetUIDeltaTimeK(float k) => _uiCoef = k;

}

