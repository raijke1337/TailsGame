using Arcatech;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Extensions = Arcatech.Extensions;

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



    #region save

    private SerializedSaveData _loadedSave;
    public SerializedSaveData GetSaveData
    {
        get
        {
            if (_loadedSave == null) SetupSaves();
            return _loadedSave;
        }
    }
    private string _savepath;
    private void SetupSaves()
    {
        _savepath = Application.dataPath + Constants.Configs.c_SavesPath;
        // TODO proper serialization

        var data = Extensions.SaveLoad.LoadSaveDataFromXML(_savepath);
        if (data == null)
        {
            _loadedSave = CreateDefaultSave();
        }
        else
        {
            _loadedSave = data;
        }

    }
    public void UpdateSaveData(string newLevel)
    {
        if (!_loadedSave.OpenedLevels.Contains(newLevel))
        {
            _loadedSave.OpenedLevels.Add(newLevel);
            Extensions.SaveLoad.SaveDataXML(_loadedSave, _savepath);
        }
    }
    public void UpdateSaveData(ItemsStringsSave data)
    {
        _loadedSave.PlayerItems = data;
        Extensions.SaveLoad.SaveDataXML(_loadedSave, _savepath);
    }

    private SerializedSaveData CreateDefaultSave()
    {

        ItemsStringsSave items = new ItemsStringsSave();
        var defcfg = GetConfigByID<UnitItemsSO>("player");
        foreach (var e in defcfg.Equipment)
        {
            items.Equips.Add(e.ID);
        }
        foreach (var i in defcfg.Inventory)
        {
            items.Inventory.Add(i.ID);
        }

        SerializedSaveData save = new SerializedSaveData(
            new List<string>
            { Constants.Configs.c_FirstLevelID },
              items
            );
        Extensions.SaveLoad.SaveDataXML(save, _savepath);

        return save;
    }


    public void OnNewGame()
    {
        CreateDefaultSave();
    }
    #endregion



    #region configs
    [SerializeField] private List<ScriptableObjectID> _dictSO;
    private void FindAllConfigs()
    {

        _dictSO = new List<ScriptableObjectID>();
        //Resources.FindObjectsOfTypeAll<ScriptableObjectID>());


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
        List<T> list = new List<T>();

        //Debug.Log($"Looking for cfg {ID} type {typeof(T)}");

        if (_dictSO.Count == 0)
        {
            FindAllConfigs();
        }

        foreach (var item in _dictSO)
        {
            if (item is T)
            {
                list.Add(item as T);
            }

        }
        try
        {
            var result = list.First(t => t.ID == ID);
            return result;
        }
        catch
        {
            Debug.LogWarning($"Couldnt find {typeof(T)} with ID {ID}");
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

    public UnitInventoryItemConfigsContainer GenerateDefaultInventory(string id)
    {
        var cfg = GetConfigByID<UnitItemsSO>(id);
        if (cfg == null) Debug.LogWarning($"No unit items SO found with ID {id}");
        return new UnitInventoryItemConfigsContainer(cfg);
    }

    #endregion


    public void SetPlayerPref(string pref, object value)
    {
        if (value is float)
        {
            PlayerPrefs.SetFloat(pref, (float)value);
        }
        if (value is int)
        {
            PlayerPrefs.SetInt(pref, (int)value);
        }
        if (value is string)
        {
            PlayerPrefs.SetString(pref, (string)value);
        }
    }
}
