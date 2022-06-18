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

[CreateAssetMenu(fileName = "New BaseWeaponConfig", menuName = "Configurations/Weapons", order = 1)]
public class BaseWeaponConfig : ScriptableObjectID
{

    public WeaponType WType;
    public int _charges;
    public List<string> TriggerIDs;
    public string SkillID;
    public int ComboValue;
    // how much combo increases per hit
    public float InternalCooldown;

}

