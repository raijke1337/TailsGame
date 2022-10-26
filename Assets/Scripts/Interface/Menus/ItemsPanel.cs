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

public class ItemsPanel : MenuPanel
{
    private SaveData saveData;

    protected override void Start()
    {
        base.Start();
        _elementsLoc.Reverse();
        for (int i = 0; i < _elementsLoc.Length; i++)
        {
            _tiles[i] = Instantiate(TilePrefab);
            _tiles[i].GetRekt.SetParent(transform);
            _tiles[i].GetRekt.anchoredPosition = _elementsLoc[i];
        }

        saveData = GameManager.GetGameManager.GetSaveData;

        for (int i = 0; i < saveData.PlayerItems.InventoryIDs.Count; i++)
        {
            _tiles[i].Content = GameManager.GetItemsHandler().
                GetItemByID<ItemBase>(saveData.PlayerItems.InventoryIDs[i]).GetContents;
            // yikes but should work
        }

    }


}

