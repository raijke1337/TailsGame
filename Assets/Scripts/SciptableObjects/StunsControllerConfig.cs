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
[CreateAssetMenu(fileName = "New Stuns COntroller Config", menuName = "Configurations/Stuns")]
public class StunsControllerConfig : ScriptableObjectID
{
    public StatValueContainer StunResistance;
    public float RegenPerSec;
    public float GracePeriod;
}

