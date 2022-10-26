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

public class EquipsPanel : MenuPanel
{
    SaveData saveData;

    protected override void Start()
    {
        base.Start();
        _tiles = GetComponentsInChildren<ItemTileComponent>();
        saveData = GameManager.GetGameManager.GetSaveData;
        if (saveData.PlayerItems.EquipmentIDs == null) return;

        for (int i = 0; i < saveData.PlayerItems.EquipmentIDs.Count;i++)
        {
            _tiles[i].Content = GameManager.GetItemsHandler().GetItemByID<ItemBase>(saveData.PlayerItems.EquipmentIDs[i]).GetContents; 
            // yikes but should work
        }

    }
}

