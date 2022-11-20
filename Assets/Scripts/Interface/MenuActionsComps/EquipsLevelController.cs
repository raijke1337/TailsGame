using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class EquipsLevelController : MonoBehaviour
{
    private SaveData saveData;

    [SerializeField] private MenuPanelTiled Items;
    [SerializeField] private EquipsPanel Equipments;

    [SerializeField] private Canvas _tooltips;
    [SerializeField] private TooltipComp _tooltipPrefab;
    private TooltipComp instantiatedTT;

    [SerializeField] private bool _showingTooltip = false;

    private PlayerUnit _player;
    private ItemsEquipmentsHandler _items;
    

    private void OnEnable ()
    {
        _player = FindObjectOfType<PlayerUnit>();
        if (_player == null)
        {
            Debug.LogError($"Player unit not found!");
        }

        if (Items == null | Equipments == null)
        {
            Debug.LogError("Set panels for "+this.gameObject.name);
            return;
        }
        saveData = GameManager.GetInstance.GetSaveData;
        _items = GameManager.GetItemsHandler;
        FillTiles(saveData);
        Items.ShowtooltipToggle += HandleTooltipShow;
        Equipments.ShowtooltipToggle += HandleTooltipShow;
        Items.TileClickToggle += Items_EquipItemToggle;
        Equipments.TileClickToggle += Equipments_EquipItemToggle;
    }

    private void FillTiles(SaveData save)
    {
        Items.CreateTilesAndSub(99); // lol, todo
        Equipments.CreateTilesAndSub(99);

        var inventory = saveData.PlayerItems.InventoryIDs;
        var equips = saveData.PlayerItems.EquipmentIDs;

        foreach (var equip in equips)
        {
            Equipments.AddTileContent(_items.GetItemCOntentByID(equip));
        }
        foreach (var item in inventory)
        {
            Items.AddTileContent(_items.GetItemCOntentByID(item));
        }

    }

    private void OnDisable()
    {
        Items.ShowtooltipToggle -= HandleTooltipShow;
        Equipments.ShowtooltipToggle -= HandleTooltipShow;
        Items.TileClickToggle -= Items_EquipItemToggle;
        Equipments.TileClickToggle -= Equipments_EquipItemToggle;
    }

    private void Equipments_EquipItemToggle(ItemContent arg)
    {
        Equipments.RemoveTileContent(arg);
        Items.AddTileContent(arg);
        UpdateItemsInSave(arg, false);
    }

    private void Items_EquipItemToggle(ItemContent arg)
    {
        Items.RemoveTileContent(arg);
        Equipments.AddTileContent(arg);
        UpdateItemsInSave(arg, true);
    }
    private void Update()
    {
        if (instantiatedTT == null)
        {
            instantiatedTT = Instantiate(_tooltipPrefab);
            var rect = instantiatedTT.GetComponent<RectTransform>();
            rect.SetParent(_tooltips.transform);
        }
        instantiatedTT.gameObject.SetActive(_showingTooltip);
        if (_showingTooltip)      
        {
            instantiatedTT.GetRect.anchoredPosition = Mouse.current.position.ReadValue();
        }
    }

    private void HandleTooltipShow(ItemContent arg1, bool arg2)
    {
        _showingTooltip = arg2;
        instantiatedTT.SetTexts(arg1);
    }

    private void UpdateItemsInSave(ItemContent cont, bool IsEquip)
    {
        if (IsEquip)
        {
            saveData.PlayerItems.EquipmentIDs.Add(cont.ID);
            saveData.PlayerItems.InventoryIDs.Remove(cont.ID);
        }
        else
        {
            saveData.PlayerItems.EquipmentIDs.Remove(cont.ID);
            saveData.PlayerItems.InventoryIDs.Add(cont.ID);
        }
        GameManager.GetInstance.UpdateSaveData();
        _player.InitInventory(_items);
    }

    public void OnBackButton()
    {
        LevelsLoaderManager.GetInstance.RequestLevelLoad(0);
    }
    public void OnStartButton()
    {
        LevelsLoaderManager.GetInstance.RequestLevelLoad();
    }




}
