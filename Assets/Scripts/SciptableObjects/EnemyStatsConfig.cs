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
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(fileName = "New EnemyStats", menuName = "Configurations/EnemyStats", order = 2)]

public class EnemyStatsConfig : ScriptableObject
{
    public string ID;
    public SerializableDictionaryBase<EnemyStatType, float> Stats;
}

