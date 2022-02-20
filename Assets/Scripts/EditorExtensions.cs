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


[CustomPropertyDrawer(typeof(StatContainer))]
public class StatContainerPropertyDrawer : PropertyDrawer
{
    private const string c_containerFieldName = "_defaultValue";
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.Slider(position, 
            property.FindPropertyRelative(c_containerFieldName), -100f, 100f, GUIContent.none);
    }
    // this allows for better look in unitstate component



}
