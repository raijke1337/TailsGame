
using Arcatech.Items;
using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

public class EquipsPanel : MenuPanelTiled
{
    [SerializeField] protected SerializedDictionary<EquipItemType, ItemTileComponent> EquipsTiles;
    public override void AddTileContent(InventoryItem content)
    {
        if (content == null) return;
        try
        {
            EquipsTiles[content.ItemType].Item = content;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"No slot for item in {this} {e}");
        }
    }

    public override void RemoveTileContent(InventoryItem content)
    {
        if (content == null) return;
        try
        {
            EquipsTiles[content.ItemType].Clear();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"No equipped item {content} {e}");
        }
    }

    public override void CreateTilesAndSub(int total)
    {
        foreach (var tile in EquipsTiles.Values)
        {
            SubscribeToTileEvents(tile, true);
        }
    }
    protected override void OnDisable()
    {
        foreach (var tile in EquipsTiles.Values)
        {
            SubscribeToTileEvents(tile, false);
        }
    }

}

