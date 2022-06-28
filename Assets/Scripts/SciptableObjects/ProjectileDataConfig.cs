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
[CreateAssetMenu(fileName = "New ProjectileConfig", menuName = "Configurations/Projectiles", order = 1)]
public class ProjectileDataConfig : ScriptableObjectID
{

    public float TimeToLive;
    public float ProjectileSpeed;
    [Range(1,999)] public int ProjectilePenetration;

}

