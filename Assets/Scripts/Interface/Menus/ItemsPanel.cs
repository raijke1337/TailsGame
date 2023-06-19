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
    ItemsEquipmentsHandler _handler;


    protected override void Start()
    {
        base.Start();
        _handler = GameManager.GetItemsHandler();
        saveData = GameManager.GetGameManager().GetSaveData();

        _elementsLoc.Reverse();

        for (int i = 0; i < _elementsLoc.Length; i++)
        {
            _tiles[i] = Instantiate(TilePrefab);
            _tiles[i].GetRekt.SetParent(transform);
            _tiles[i].GetRekt.anchoredPosition = _elementsLoc[i];
            _tiles[i].name = "Tile " + i;
        }

        var items = _handler.GetContentsByID(saveData.BackPackIDs);
        for (int i = 0; i < items.Length; i++)
        {
            _tiles[i].Content = items[i];
        }
        SubscribeToTiles(true);

    }


}

