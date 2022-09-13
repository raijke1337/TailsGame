//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    private SaveData _saveData;
    private static ItemsEquipmentsHandler _itemshandler;
    private static GameManager instance;

    [SerializeField] private InventorySO _defaultInventory;


    public static GameManager GetGameManager() => instance;
    public static ItemsEquipmentsHandler GetItemsHandler()
    {
        if (_itemshandler == null )
        {
            _itemshandler = new ItemsEquipmentsHandler(new List<IInventoryItem>(Extensions.GetAssetsOfType<ItemBase>(Constants.Combat.c_ItemPrefabsPath)));
        }
        return _itemshandler;
    }

    private void Awake()
    {
        instance = this;
        LoadDataFromJSON();
    }

    private void OnDisable()
    {
        SaveDataToJSON();
    }

    public SaveData GetSaveData()
    {
        if (_saveData == null) LoadDataFromJSON();
        return _saveData;
    }

    private void LoadDataFromJSON()
    {
        string json = string.Empty;
        var path = string.Concat(Application.dataPath, "/save.json");
        try
        {
            StreamReader sr = new StreamReader(path);
        }
        catch (Exception ex)
        {
            Debug.Log($" No save file {ex.Message}");
            SaveDataToJSON();
            // it is null so we just create a new one 
        }



        using (StreamReader sr = new StreamReader(path))
        {
            using (JsonReader reader = new JsonTextReader(sr))
            {
                json = reader.ReadAsString();
            }
        }
        _saveData = (SaveData)JsonUtility.FromJson(json,typeof(SaveData));
        AssetDatabase.Refresh();
    }

    private void SaveDataToJSON()
    {
        string data;

        if (_saveData != null)
        {
            //data = JsonConvert.SerializeObject(_saveData);
            // this cant serialize gameobjects
            data = JsonUtility.ToJson(_saveData);

            Debug.Log($"Saving existing file; {data}");
        }
        else
        {
            var all = Extensions.GetAssetsOfType<ItemBase>(Constants.Combat.c_ItemPrefabsPath);
            var backp = from item in all
                        where _defaultInventory.itemIDs.Contains(item.GetID)
                        select item;
            var b  = backp.ToList();


            data = JsonUtility.ToJson(new SaveData(b)); // case : no save
            Debug.Log($"Saving new file; {data}");
        }

        var path = string.Concat(Application.dataPath, "/save.json");
        using (StreamWriter stream = new StreamWriter(path))
        {
            using (JsonWriter writer = new JsonTextWriter(stream))
            {
                writer.WriteValue(data);
            }
        }
        AssetDatabase.Refresh();
    }
}
[Serializable]
public class SaveData
{
    public int LastLevelIndex;
    public List<ItemBase> BackPack;
    public List<IEquippable> Equipments;
    public SaveData(int lastLevelIndex, List<ItemBase> backPack, List<IEquippable> equipments)
    {
        LastLevelIndex = lastLevelIndex;
        BackPack = backPack;
        Equipments = equipments;
    }
    public SaveData(List<ItemBase> backPack)
    {
        BackPack = backPack;
        Equipments = new List<IEquippable>();
    }
}


