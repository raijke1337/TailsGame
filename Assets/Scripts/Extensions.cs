using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class Extensions
{/// <summary>
/// get assets of T type
/// </summary>
/// <typeparam name="T">any class</typeparam>
/// <param name="path">refer to Constants</param>
/// <param name="editorMode">look in subfolders instead</param>
/// <returns>list of assets in specified folder</returns>
    public static List<T> GetAssetsFromPath<T> (string path,bool editorMode = false) where T: class
    {
        List<T> result = new List<T>();
        List<string> workPaths = new List<string>();

        if (!editorMode)
        {
            workPaths.Add(path);
        }
        if (editorMode)
        {
            var folders = Directory.GetDirectories(Application.dataPath + path);
            List<string> fixedFolders = new List<string>();
            foreach (var folder in folders)
            {
                string foldername = folder.Substring(folder.LastIndexOf("/") + 1);
                workPaths.Add(path + foldername + "/");
            }
        }
        List<string> filesAtPaths = new List<string>();
        foreach (var folder in workPaths)
        {
            var res = (Directory.GetFiles(Application.dataPath + folder));
            filesAtPaths.AddRange(res);
        }        

        foreach (string found in filesAtPaths)
        {
            var replace = Application.dataPath;
            var foundRelativ = found.Replace(replace.ToString(), "Assets");

            var file = AssetDatabase.LoadAssetAtPath(foundRelativ, typeof(T));
            if (file is T)
            {
                result.Add(file as T);
            }
        }
        return result;
    }

    public static float GetRandomFloat(float max)
    {
        return Random.Range(0, max);
    }

}
