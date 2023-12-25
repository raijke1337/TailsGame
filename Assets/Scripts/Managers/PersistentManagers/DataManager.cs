using Arcatech;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Scenes;
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
        _serializer = new SavesSerializer();
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    #endregion



    #region saves

    private SavesSerializer _serializer;

    private GameSave _loadedSave;
    public GameSave GetSaveData
    {
        get
        {
            if (_loadedSave == null && _serializer.TryLoadSaveData(out var s))
            {

            }
            return _loadedSave;
        }
    }
    public void UpdateSaveData(SceneContainer newLevel)
    {
        if (!_loadedSave.OpenedLevels.Contains(newLevel))
        {
            _loadedSave.OpenedLevels.Add(newLevel);
            Extensions.SavesSerializer.SaveDataXML(_loadedSave, _savepath);
        }
    }
    public void UpdateSaveData(ItemsStringsSave data)
    {
        _loadedSave.PlayerItems = data;
        Extensions.SavesSerializer.SaveDataXML(_loadedSave, _savepath);
    }

    private GameSave CreateDefaultSave()
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

        var lvs = GameManager.Instance.GetDefaultGameLevels;
        var list = new List<string>();
        foreach (var l in lvs)
        {
            list.Add(l.ID);
        }

        SerializedSaveData save = new SerializedSaveData(list,items);
        Extensions.SavesSerializer.SaveDataXML(save, _savepath);

        return new GameSave(save);

    }


    public void OnNewGame()
    {
        _loadedSave = CreateDefaultSave ();
        _serializer.
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
    public UnitInventoryItemConfigsContainer GenerateDefaultInventory(UnitItemsSO cfg)
    {
        return new UnitInventoryItemConfigsContainer(cfg);
    }

    #endregion

    #region prefs
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

    #endregion
}
