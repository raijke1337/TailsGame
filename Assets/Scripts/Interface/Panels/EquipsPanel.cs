using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

public class EquipsPanel : MenuPanelTiled
{
    [SerializeField] protected SerializableDictionaryBase<EquipItemType, ItemTileComponent> EquipsTiles;
    public override void AddTileContent(ItemContent content)
    {
        if (content == null) return;
        try
        {
            EquipsTiles[content.ItemType].Content = content;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"No slot for item in {this} {e}");
        }
    }

    public override void RemoveTileContent(ItemContent content)
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

