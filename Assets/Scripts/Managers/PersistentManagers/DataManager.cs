using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region SingletonLogic

    public static DataManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    #endregion


    private void Start()
    {
        FindAllConfigs();
        SetupSaves();
    }

    #region save

    private SaveData _loadedSave;
    public SaveData GetSaveData { get => _loadedSave; }
    private string _savepath;
    private void SetupSaves()
    {
        _savepath = Application.dataPath + Constants.Configs.c_SavesPath;

        var data = Extensions.SaveLoad.LoadSaveDataFromXML(_savepath);
        if (data == null)
        {
            CreateDefaultSave();
        }
        else
        {
            _loadedSave = data;
        }

        UpdateSaveData();
    }
    public void UpdateSaveData()
    {
        Extensions.SaveLoad.SaveDataXML(_loadedSave, _savepath);
    }

    public void CreateDefaultSave()
    {
        Extensions.SaveLoad.SaveDataXML(new SaveData(new List<string> { Constants.Configs.c_FirstLevelID },
            new UnitInventoryItems(GetConfigByID<UnitItemsSO>("player"))), _savepath);
        _loadedSave = Extensions.SaveLoad.LoadSaveDataFromXML(_savepath);
    }



    #endregion

    #region configs
    [SerializeField] private List<ScriptableObjectID> _dictSO;
    private void FindAllConfigs()
    {
        _dictSO = new List<ScriptableObjectID>();

        string path = Constants.Configs.c_AllConfigsPath;
        string appPath = Application.dataPath;
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

            var file = AssetDatabase.LoadAssetAtPath<ScriptableObjectID>(foundRelativ);
            if (file is ScriptableObjectID)
            {
                _dictSO.Add(file);
            }
        }
        //Debug.Log($"{this} loaded total {_dictSO.Count} config objects");

    }


    /// <summary>
    /// get configs of T type
    /// </summary>
    /// <typeparam name="T">config SO</typeparam>
    /// <param name="path">refer to Constants</param>
    /// <param name="includeSubDirs">look in subfolders </param>
    /// <returns>list of assets in specified folder</returns>
    public T GetConfigByID<T>(string ID = "default") where T : ScriptableObjectID
    {
        if (ID == "")
        {
            Debug.Log($"ID was empty");
            return null;
        }
        if (ID == "default")
        {
            //Debug.Log($"Loading default config of type {typeof(T)}");
        }
        try
        {
            List<T> list = new List<T>();
            foreach (var config in _dictSO)
            {
                if (config is T t)
                {
                    list.Add(t);
                }
            }
            return list.First(t => t.ID == ID);
        }
        catch (InvalidOperationException e)
        {
            Debug.Log($"No config of type {typeof(T)} found by ID {ID} ; {e.Message}");
            return null;
        }
    }

    public T[] GetAssetsOfType<T>(string path) where T : class
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
        return all.ToArray();
    }
    #endregion

}

[XmlRoot("GameSave"), Serializable]
public class SaveData
{
    public List<string> OpenedLevels;
    public UnitInventoryItems PlayerItems;

    public SaveData(List<string> levels, UnitInventoryItems items)
    {
        OpenedLevels = new List<string>();
        foreach (var l in levels)
        {
            if (!OpenedLevels.Contains(l)) OpenedLevels.Add(l);
        }
        PlayerItems = items;
    }
    public SaveData()
    {
        OpenedLevels = new List<string>();
    }

}

