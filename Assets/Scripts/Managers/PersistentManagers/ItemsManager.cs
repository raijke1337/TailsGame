using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    #region SingletonLogic

    public static ItemsManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    #endregion

    private Dictionary<string, IInventoryItem> AllItems;


    private void Start()
    {
        AllItems = new Dictionary<string, IInventoryItem>();
        var items = new List<IInventoryItem>(DataManager.Instance.GetAssetsOfType<ItemBase>(Constants.PrefabsPaths.c_ItemPrefabsPath));
        foreach (var i in items)
        {
            AllItems[i.GetID] = i;
        }
    }

    public T GetItemByID<T>(string id) where T : IInventoryItem
    {
        return (T)AllItems[id];
    }
    public ItemContent GetItemContentByID(string ID)
    {
        return AllItems[ID].GetContents;
    }


    [Serializable]
    public class DroppableItem
    {
        public ItemBase Item;
        [Range(0, 100)] public float Chance;
    }



}
