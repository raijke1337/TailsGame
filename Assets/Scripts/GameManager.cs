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
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    private SaveData saveData;

    
    private void Start()
    {
        saveData = LoadDataFromJSON();
        Debug.Log($"Loaded save, levels: {saveData.LastLevelIndex}, items: {saveData.Items.Length}");
    }


    [MenuItem("Saves/Load existing save json")]
    public static SaveData LoadDataFromJSON()
    {
        string json = string.Empty;
        var path = string.Concat(Application.dataPath, "/", "test.json");
        using (StreamReader sr = new StreamReader(path))
        {
            using (JsonReader reader = new JsonTextReader(sr))
            {
                json = reader.ReadAsString();
            }
        }
        var data = JsonConvert.DeserializeObject<SaveData>(json);
        return data;
    }

    [MenuItem("Saves/Create new save json")]
    public static void SaveDataToJSON()
    {
        var data = JsonConvert.SerializeObject(new SaveData { LastLevelIndex = 3, Items = new ItemBase[3]});
        // todo

        var path = string.Concat(Application.dataPath, "/", "test.json");
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
public struct SaveData
{
    public int LastLevelIndex;
    public ItemBase[] Items;
}

