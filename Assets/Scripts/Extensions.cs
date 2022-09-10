
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public static class Extensions
{
    #region IT WORKS
    /// <summary>
    /// get configs of T type
    /// </summary>
    /// <typeparam name="T">any class</typeparam>
    /// <param name="path">refer to Constants</param>
    /// <param name="includeSubDirs">look in subfolders </param>
    /// <returns>list of assets in specified folder</returns>
    public static T GetConfigByID<T>(string ID) where T : ScriptableObjectID
    {
        //Debug.Log($"Loading config {typeof(T)} with ID {ID}");
        if (ID == "")
        {
            Debug.Log($"ID was empty");
            return null;
        }
        string path = Constants.Configs.c_AllConfigsPath;

        string appPath = Application.dataPath;

        List<T> all = new List<T>();

        List<string> files = new List<string>();

        Stack<string> workPaths = new Stack<string>(new string[1] { path });
        while (workPaths.Count > 0)
        {
            string currFolder = workPaths.Pop();
            string[] foundSubfolders = Directory.GetDirectories(appPath + currFolder);
            string[] foundFiles = Directory.GetFiles(appPath + currFolder);
            files.AddRange(foundFiles);

            foreach (string foundpath in foundSubfolders)
            {
                int index = foundpath.IndexOf(path);
                var foldername = foundpath.Substring(index);
                workPaths.Push(foldername + "/");
            }
        }

        foreach (string found in files)
        {
            var foundRelativ = found.Replace(appPath.ToString(), "Assets");

            var file = AssetDatabase.LoadAssetAtPath(foundRelativ, typeof(T));
            if (file is T)
            {
                all.Add(file as T);
            }
        }
        try
        { 
            return all.First(t => t.ID == ID);
        }
        catch (InvalidOperationException e)
        {
            Debug.Log($"No config of type {typeof(T)} found by ID {ID} ; {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// get assets of T type
    /// </summary>
    /// <typeparam name="T">any class</typeparam>
    /// <param name="path">refer to Constants</param>
    /// <param name="includeSubDirs">look in subfolders </param>
    /// <returns>list of assets in specified folder</returns>
    public static IEnumerable<T> GetAssetsOfType<T>(string path) where T : class
    {
        string appPath = Application.dataPath;

        List<T> all = new List<T>();

        List<string> files = new List<string>();

        Stack<string> workPaths = new Stack<string>(new string[1] { path });
        while (workPaths.Count > 0)
        {
            string currFolder = workPaths.Pop();
            string[] foundSubfolders = Directory.GetDirectories(appPath + currFolder);
            string[] foundFiles = Directory.GetFiles(appPath + currFolder);
            files.AddRange(foundFiles);

            foreach (string foundpath in foundSubfolders)
            {
                int index = foundpath.IndexOf(path);
                var foldername = foundpath.Substring(index);                
                workPaths.Push(foldername + "/");
            }
        }

        foreach (string found in files)
        {
            var foundRelativ = found.Replace(appPath.ToString(), "Assets");

            var file = AssetDatabase.LoadAssetAtPath(foundRelativ, typeof(T));
            if (file is T)
            {
                all.Add(file as T);
            }
        }
        return all; 
    }
#endregion

    #region JSON


    #endregion

    #region tiles
    public static Vector2[] GetTilePositions(RectTransform assignedSpace, Vector2 tileOffsets, Vector2 desiredTileSize)
    {

        var fieldHeight = assignedSpace.rect.height;
        var fieldWidth = assignedSpace.rect.width;

        var _tileWidth = desiredTileSize.x;
        var _tileHeight = desiredTileSize.y;

        float _tileTotalW = _tileWidth + tileOffsets.x;
        float _tileTotalH = _tileHeight + tileOffsets.y;

        int totalHorizontal = ((int)(fieldWidth / _tileTotalW));
        int totalVertical = ((int)(fieldHeight / _tileTotalH));

        Vector2 adjustedOffset = new Vector2(tileOffsets.x, -tileOffsets.y); // move to right and down
        Vector2[] points = new Vector2[totalHorizontal*totalVertical];

        int currentIndex = 0;

        for (int i = 0, j = 0; i < totalVertical; i++)
        {
            while (j < totalHorizontal)
            {
                Vector2 point = new Vector2(adjustedOffset.x + (adjustedOffset.x * j) + (_tileWidth * j), adjustedOffset.y+(adjustedOffset.y * i) + (-_tileHeight * i));
                points[currentIndex] = point;
                j++;
                currentIndex++;
            }
            j = 0;            
        }

        currentIndex = 0;
        return points;
    }
    #endregion

    #region random
    public static System.Random Randoms = new System.Random();
    /// <summary>
    /// Return a shuffled list using Fisher-Yates method
    /// </summary>
    /// <typeparam name="T">anything</typeparam>
    /// <param name="input">List<></param>
    /// <returns>same list but mixed</returns>
    public static List<T> ShuffledList<T> (List<T> input)
    {
        for (int i = input.Count - 1; i >= 1; i--)
        {
            int j = Randoms.Next(i + 1);
            T tmp = input[i];
            input[i] = input[j];
            input[j] = tmp;
        }
        return input;
    }
    #endregion
}

