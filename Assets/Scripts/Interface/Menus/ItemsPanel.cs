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
    [SerializeField] private float tileOffsetPx = 10f;
    [SerializeField] private ItemTileComponent TilePrefab;

    [SerializeField] private RectTransform _equippedPanel;
    [SerializeField] private RectTransform _bagPanel;

    private IEnumerable <ItemContent> _equip;
    private IEnumerable <ItemContent> _bag;


    [SerializeField] private ItemsEquipmentsHandler inventoryManager;

    //public override void OnToggle()
   // {
        //if (inventoryManager == null) inventoryManager = FindObjectOfType<ItemsEquipmentsHandler>(); 
        //base.OnToggle();

        //_equip = inventoryManager.GetEquippedItems;
        //_bag = inventoryManager.GetBagItems;
        //var equipsTiles = PlaceTiles(_equippedPanel, TilePrefab);
        //var baggedTiles = PlaceTiles(_bagPanel, TilePrefab);

        //for (int i = 0; i < equipsTiles.Length; i++)
        //{ 
        //    var tile = equipsTiles[i];
        //    tile.Content = _equip.ElementAt(i);
        //    tile.ItemClickedEvent += Tile_ItemClickedEvent;
        //}
        //for (int i = 0; i < _bag.Count(); i++)
        //{
        //    var tile = baggedTiles[i];
        //    tile.Content = _bag.ElementAt(i);
        //    tile.ItemClickedEvent += Tile_ItemClickedEvent;
        //}

        // todo
   // }

    private void Tile_ItemClickedEvent(ItemContent arg)
    {
        Debug.Log($"{arg.DisplayName} was clicked");
    }


    // logic from puzzle updated
    //private ItemTileComponent[] PlaceTiles(RectTransform panel, ItemTileComponent tile)
    //{
    //    List<ItemTileComponent> list = new List<ItemTileComponent>();

    //    var fieldHeight = panel.rect.height;
    //    var fieldWidth = panel.rect.width;
    //    var _tileWidth = tile.GetRekt.rect.width;
    //    var _tileHeight = tile.GetRekt.rect.height;


    //    int tilesHor = Mathf.RoundToInt(fieldWidth / _tileWidth);
    //    int tilesVert = Mathf.RoundToInt(fieldHeight / _tileHeight);


    //    var allTilesWidth = _tileWidth * tilesHor;
    //    var allTilesHeight = _tileHeight * tilesVert;

    //    Vector2 DefaultOffset = new Vector2(tileOffsetPx, -tileOffsetPx);



    //    for (int i = 0; i < tilesHor; i++)
    //    {
    //        for (int j = 0; j < tilesVert; j++)
    //        {
    //            var item = Instantiate(tile); list.Add(item);

    //            var rect = item.GetRekt;
    //            item.transform.SetParent(panel, false);

    //            rect.anchoredPosition += DefaultOffset;
    //            rect.anchoredPosition += new Vector2(i * (tileOffsetPx + _tileWidth), j * (tileOffsetPx - _tileHeight));

    //            item.name = $"Tile {i},{j}";
    //        }
    //    }

    //    return list.ToArray();

    //}

}

