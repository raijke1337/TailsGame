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

[CreateAssetMenu(fileName = "New BaseSkillConfig",menuName ="Configurations/Skills")]
public class BaseSkillSettings : ScriptableObject
{
    public string ID;
    public CombatActionType SkillType;
    public float Recharge;

    public Sprite Icon;
}

