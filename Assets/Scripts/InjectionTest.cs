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

namespace debug
{
    public class InjectionTest : MonoBehaviour
    {
        [Inject] public PlayerUnit _player;
        [Inject] public StatsUpdatesHandler _statsH;
        [Inject] public TriggersManager _triggers;
    }

}