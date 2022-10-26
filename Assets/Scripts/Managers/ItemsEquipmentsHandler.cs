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

    public ItemsEquipmentsHandler(List<IInventoryItem> items)
    {
        AllItems = new Dictionary<string, IInventoryItem>();
        foreach (var i in items)
        {
            AllItems[i.GetID] = i;
        }
    }

    public T GetItemByID<T>(string id) where T : IInventoryItem
    {
        return (T)AllItems[id];
    }


    [Serializable]
    public class DroppableItem
    {
        public ItemBase Item;
        [Range(0, 100)] public float Chance;
    }


}



