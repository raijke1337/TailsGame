//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
using Zenject.Asteroids;

public class GameManager : SingletonManagerBase
{
    private SaveData _loadedSave;
    public SaveData GetSaveData { get => _loadedSave; } 
    private string _savepath;


    private GameMode _mode;
    public GameMode Mode
    {
        get
        {
            return _mode;   
        }

        set
        {
            _mode = value;
            Debug.Log($"Game mode set to:{value}");
            OnGameModeChanged?.Invoke(value);
        }    
    }
    public SimpleEventsHandler<GameMode> OnGameModeChanged;

    public override void SetupManager()
    {

        _savepath = Application.dataPath + Constants.Configs.c_SavesPath;

        var data = Extensions.SaveLoad.LoadSaveDataFromXML(_savepath);
        if (data == null)
        {
            data = CreateDefaultSave();
            UpdateSaveData();
        }
        _loadedSave = (data);

    }

    public void UpdateSaveData()
    {
        Extensions.SaveLoad.SaveDataXML(_loadedSave, _savepath);
    }

    private SaveData CreateDefaultSave()
    {
        SaveData data = new SaveData(2, new UnitInventoryItems("player")); // 2 is first playable level
        return data;
    }

    #region itemshandler
    private static ItemsEquipmentsHandler _itemshandler;
    public static ItemsEquipmentsHandler GetItemsHandler
    {
        get
        {
            if (_itemshandler == null)
            {
                _itemshandler = new ItemsEquipmentsHandler(new List<IInventoryItem>(Extensions.GetAssetsOfType<ItemBase>(Constants.PrefabsPaths.c_ItemPrefabsPath)));
            }
            return _itemshandler;
        }
    }
    #endregion

    #region SingletonLogic

    protected static GameManager _instance = null;
    public override void InitSingleton()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance == this) Destroy(gameObject); // remove copies just in case
        SetupManager();
        Mode = GameMode.Menus;
    }
    public static GameManager GetInstance
    {
        get
        {
            return _instance;
        }
    }
    #endregion
}


