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

public class CrosshairComponent : MonoBehaviour
{
    public void SetScreenPosition(Vector3 position) => transform.position = position;

    private SpriteRenderer _renderer;
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>(); 
    }
    public void SetCrosshair(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }
}

