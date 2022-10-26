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

public class GameManager : MonoBehaviour
{
    private SaveData _loadedSave;
    public SaveData GetSaveData { get => _loadedSave; } 


    private string _savepath;
    private static ItemsEquipmentsHandler _itemshandler;
    private static GameManager instance;

    public static GameManager GetGameManager
    {
        get 
        {
            return instance;
        }

    }
    public static ItemsEquipmentsHandler GetItemsHandler()
    {
        if (_itemshandler == null)
        {
            _itemshandler = new ItemsEquipmentsHandler(new List<IInventoryItem>(Extensions.GetAssetsOfType<ItemBase>(Constants.PrefabsPaths.c_ItemPrefabsPath)));
        }
        return _itemshandler;
    }

    private void Awake()
    {
        instance = this;
        _savepath = Application.dataPath + Constants.Configs.c_SavesPath;

        var data = Extensions.SaveLoad.LoadSaveDataFromXML(_savepath);
        if (data == null)
        {
            data = CreateDefaultSave();
        }
        _loadedSave = (data);
    }

    private void OnDisable()
    {
        Extensions.SaveLoad.SaveDataXML(_loadedSave, _savepath);
    }

    private SaveData CreateDefaultSave()
    {
        SaveData data = new SaveData(0, new UnitInventoryItems("player")); //todo
        return data;
    }
}

[XmlRoot("GameSave"),Serializable]
public class SaveData
{
    public int LastLevelIndex;
    public UnitInventoryItems PlayerItems;

    public SaveData(int lastLevelIndex, UnitInventoryItems items)
    {
        LastLevelIndex = lastLevelIndex;
        PlayerItems = items;
    }
    public SaveData (SaveData data)
    {
        LastLevelIndex = data.LastLevelIndex; PlayerItems = data.PlayerItems;
    }
    public SaveData() { LastLevelIndex = 0; }
}


