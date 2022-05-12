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
using Assets.Scripts.Units;

[CustomPropertyDrawer(typeof(ComboController))]
public class ComboControllerProperyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.LabelField(position, label, new GUIContent("TODO"));
    }
}

#region TODO
[CustomPropertyDrawer(typeof(DodgeController))]
public class DodgeControllerProperyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.LabelField(position, label, new GUIContent("TODO"));
    }
}

[CustomPropertyDrawer(typeof(ShieldController))]
public class ShieldControllerProperyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.LabelField(position, label, new GUIContent("TODO"));
    }
}

[CustomPropertyDrawer(typeof(SkillsController))]
public class SkillsControllerProperyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.LabelField(position, label, new GUIContent("TODO"));
    }
}
[CustomPropertyDrawer(typeof(StunnerComponent))]
public class StunnerComponentProperyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.LabelField(position, label, new GUIContent("TODO"));
    }
}
#endregion