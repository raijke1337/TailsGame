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

public class EquipsPanel : MenuPanel
{
    SaveData _saveData;
    ItemsEquipmentsHandler _handler;
    [SerializeField] SerializableDictionaryBase<EquipItemType, ItemTileComponent> _panels; 


    protected override void Start()
    {
        base.Start();
        _tiles = GetComponentsInChildren<ItemTileComponent>();
        _saveData = GameManager.GetGameManager().GetSaveData();
        _handler = GameManager.GetItemsHandler();
        SubscribeToTiles(true);
    }


}

