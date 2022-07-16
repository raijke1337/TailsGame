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

public class PlayerInventoryManager : MonoBehaviour
{
    public GameManager GameManager { get; set; }
    [SerializeField] private List<ItemContent> BagItems;

    private PlayerUnit _player;
    private UnitInventoryComponent _playerInv;


    private void Start()
    {
        _player = GameManager.GetPlayer;
        _playerInv = _player.Inventory;
    }
    public void OnEquip(ItemContent item)
    {
        _playerInv.EquipItem(item,true);
        BagItems.Remove(item);
    }
    public void OnUnequip(ItemContent item)
    {
        _playerInv.EquipItem(item, false);
        BagItems.Add(item);
    }
    public IEnumerable<ItemContent> GetBagItems => BagItems;
    public IEnumerable<ItemContent> GetEquippedItems => _playerInv.GetEquippedItems();

}

