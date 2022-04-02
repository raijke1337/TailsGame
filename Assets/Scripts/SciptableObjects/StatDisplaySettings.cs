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
[CreateAssetMenu(menuName = "Configurations/StatDisplaySetting/")]
public class StatDisplaySettings : ScriptableObject
{
    public Text text;
    public Image filler;
    public Color maxCol;
    public Color minCol;

    public string settingID;
}

