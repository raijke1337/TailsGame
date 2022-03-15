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

public static class Extensions
{/// <summary>
/// get assets of T type
/// </summary>
/// <typeparam name="T">any class</typeparam>
/// <param name="path">path starts with / and ends without one</param>
/// <returns></returns>
    public static List<T> GetAssetsFromPath<T> (string path) where T: class
    {
        List<T> list = new List<T>();
        string[] files = Directory.GetFiles(Application.dataPath + path);

        string relativePath = "Assets" + path;

        foreach (string loc in files)
        {
            string filePath;
            int index = loc.LastIndexOf(@"\");
            // remove everything before the last \
            filePath = relativePath + loc.Substring(index);
            filePath.Replace(@"\", @"/");

            var file = AssetDatabase.LoadAssetAtPath(filePath, typeof(T));
            if (file is T)
            {
                list.Add(file as T);
            }
        }
        return list;
    }
}
