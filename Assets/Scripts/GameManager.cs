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
    [SerializeField] private int[] sceneIndexes; // if scene != gameplay player.disablecontrols  TODO
    [SerializeField] private bool NoPlayerControls = true;

    private SaveData _saveData;
    private static ItemsEquipmentsHandler _itemshandler;
    private static GameManager instance;
    private static UnitsManager _units;

    [SerializeField] private InventorySO _defaultInventory;


    public static GameManager GetGameManager() => instance;
    public static ItemsEquipmentsHandler GetItemsHandler()
    {
        if (_itemshandler == null )
        {
            _itemshandler = new ItemsEquipmentsHandler(Extensions.GetAssetsOfType<ItemBase>(Constants.Combat.c_ItemPrefabsPath));
        }
        return _itemshandler;
    }

    private void Awake()
    {
        instance = this;
        _units = GetComponent<UnitsManager>();
        if (_units == null) Debug.LogError($"{this.gameObject} is missing the {typeof(UnitsManager)} component");
        LoadDataFromJSON();
    }

    private void OnDisable()
    {
        SaveDataToJSON();
    }
    private void Start()
    {
        _units.GetPlayerUnit.LockControls(NoPlayerControls);
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
            //var all = Extensions.GetAssetsOfType<ItemBase>(Constants.Combat.c_ItemPrefabsPath);
            //var backp = from item in all
            //            where _defaultInventory.itemIDs.Contains(item.GetID)
            //            select item;
            //var b  = backp.ToList();


            data = JsonUtility.ToJson(new SaveData(_defaultInventory.itemIDs)); // case : no save
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
    //public List<ItemBase> BackPack;
    //public List<IEquippable> Equipments; // cant be properly serialized afaik
    public string[] BackPackIDs;
    public string[] InventoryIDs;


    public SaveData(int lastLevelIndex, List<ItemBase> backPack, List<IEquippable> equipments)
    {
        LastLevelIndex = lastLevelIndex;

        var eqip = equipments.ToArray();
        string[] eqID = new string[eqip.Count()];
        for (int i = 0; i < eqip.Length; i++)
        {
            eqID[i] = eqip[i].GetContents.ID; // dont like this
        }
        var bckp = backPack.ToArray();
        string[] bkID = new string[bckp.Count()];
        for (int i = 0; i < bckp.Length; i++)
        {
            bkID[i] = bckp[i].GetContents.ID;
        }
        BackPackIDs = bkID;
        InventoryIDs = eqID;
    }


    public SaveData(IEnumerable<string> backPack)
    {
        BackPackIDs = backPack.ToArray();
    }
}


