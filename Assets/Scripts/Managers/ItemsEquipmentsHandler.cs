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
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class ItemsEquipmentsHandler
{

    private Dictionary <string,IInventoryItem> AllItems;

    public ItemsEquipmentsHandler(ItemBase[] items)
    {
        AllItems = new Dictionary<string, IInventoryItem>();
        foreach (var i in items)
        {
            AllItems[i.GetID] = i;
        }
    }

    public IInventoryItem GetItemByID(string id) => AllItems[id];

    public T GetItemByID<T>(string id) where T : ItemBase
    {
        if (AllItems.ContainsKey(id)) // todo test
        return AllItems[id] as T;
        else return null;
    }
    public ItemContent[] GetContentsByID(IEnumerable <string> ids)
    {
        var _ids = ids.ToArray();
        ItemContent[] result = new ItemContent[_ids.Length];

        for (int i = 0;i < _ids.Length;i++)
        {
            result[i] = AllItems[_ids[i]].GetContents;
        }

        return result;
    }


    public class DroppableItem
    {
        public ItemBase Item;
        [Range(0, 100)] public float Chance;
    }


}



