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

public class EventSystemBandaidController : MonoBehaviour
{
    [Inject]
    private PlayerUnit _player;

    private void LateUpdate()
    {
        gameObject.transform.position = _player.transform.position;
    }
}

