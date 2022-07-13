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

public class InventoryManager : MonoBehaviour, IStatsComponentForHandler
{
    // temporary solution, equip everything from here. TODO
    public ItemContent[] ItemsToEquip;

    // storage
    public ItemBase[] Bag;
    // item receivers
    private List<IUsesItems> _slots = new List<IUsesItems>();

    public bool IsReady => ItemsToEquip.Count() > 0;

    public void AddItemUser(IUsesItems slot) => _slots.Add(slot);


    private void OnEnable()
    {
        foreach (var item in ItemsToEquip)
        {
            item.ContentItem.ItemContents = item;
        }
    }

    public void UpdateInDelta(float deltaTime)
    { 
    }

    public void SetupStatsComponent()
    {
        foreach (var user in _slots)
        {
            foreach (var item in ItemsToEquip)
            {
                user.LoadItem(item.ContentItem);
                // todo
            }
        }
    }
}


