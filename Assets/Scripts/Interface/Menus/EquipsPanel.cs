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
        saveData = GameManager.GetGameManager().GetSaveData();
        if (saveData.Equipments == null) return; // also todo

        for (int i = 0; i < saveData.Equipments.Count;i++)
        {
            _tiles[i].Content = saveData.Equipments[i].GetContents; 
        }

    }
}

