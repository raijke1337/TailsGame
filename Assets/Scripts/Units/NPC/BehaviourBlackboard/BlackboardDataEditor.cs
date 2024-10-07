using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Arcatech.BlackboardSystem
{
    [CustomEditor(typeof(BlackboardData))]
    public class BlackboardDataEditor:Editor
    {
        ReorderableList entryList;

        private void OnEnable()
        {
            entryList = new ReorderableList(serializedObject, serializedObject.FindProperty("entries"),
                true, true, true, true)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(new UnityEngine.Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight), "Key");
                    EditorGUI.LabelField(new UnityEngine.Rect(rect.x + rect.width * 0.3f, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight), "Type");
                    EditorGUI.LabelField(new UnityEngine.Rect(rect.x + rect.width * 0.6f + 5, rect.y, rect.width * 0.4f, EditorGUIUtility.singleLineHeight), "Value");
                }
            };
            entryList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = entryList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                var keyn = element.FindPropertyRelative("Key");
                var valtype = element.FindPropertyRelative("ValueType");
                var val = element.FindPropertyRelative("Value");

                var keynRect = new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight);
                var valtypeRect = new Rect(rect.x + rect.width * 0.3f, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight);
                var valrect = new Rect(rect.x + rect.width * 0.6f, rect.y, rect.width * 0.4f, EditorGUIUtility.singleLineHeight);

                EditorGUI.PropertyField(keynRect, keyn, GUIContent.none);
                EditorGUI.PropertyField(valtypeRect, valtype, GUIContent.none);


                switch ((AnyValue.ValueType)valtype.enumValueIndex)
                {
                    case AnyValue.ValueType.Int:
                        var i = val.FindPropertyRelative("intVal");
                        EditorGUI.PropertyField(valrect, i, GUIContent.none);
                        break;
                    case AnyValue.ValueType.Float:
                        var f = val.FindPropertyRelative("floatVal");
                        EditorGUI.PropertyField(valrect, f, GUIContent.none);
                        break;
                    case AnyValue.ValueType.Bool:
                        var b = val.FindPropertyRelative("boolVal");
                        EditorGUI.PropertyField(valrect, b, GUIContent.none);    
                        break;
                    case AnyValue.ValueType.String:
                        var s = val.FindPropertyRelative("stringVal");
                        EditorGUI.PropertyField(valrect, s, GUIContent.none);
                        break;
                    case AnyValue.ValueType.Vector3:
                        var v = val.FindPropertyRelative("vectorVal");
                        EditorGUI.PropertyField(valrect, v, GUIContent.none);
                        break;
                        default:
                        throw new NotImplementedException(); 
                }
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            entryList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
