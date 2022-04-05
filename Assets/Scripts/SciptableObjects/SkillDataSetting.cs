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

[CreateAssetMenu(fileName = "New SkillDataSetting", menuName = "Configurations/Skills")]
public class SkillDataSetting : ScriptableObject
{
    public string ID;

    public float Recharge;
    public CombatActionType SkillType;

    public Sprite Icon;
}


