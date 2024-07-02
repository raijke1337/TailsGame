using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Managers.Save;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Items
{
    [Serializable]
    public class UnitInventoryItemConfigsContainer
    {
        [SerializeField] public List<ItemSO> Equipment;
        [SerializeField] public List<ItemSO> Inventory;

        // used to load items from save
        public UnitInventoryItemConfigsContainer(UnitInventoryItemConfigsContainer refe)
        {
            Equipment = new List<ItemSO>(refe.Equipment);
            Inventory = new List<ItemSO>(refe.Inventory);
        }

        // used for default inventory load
        public UnitInventoryItemConfigsContainer(UnitItemsSO cfg)
        {
            Equipment = new List<ItemSO>(cfg.Equipment);
            Inventory = new List<ItemSO>(cfg.Inventory);
        }
        // used to pack save data
        public UnitInventoryItemConfigsContainer(List<Item> equipment, List<Item> inventory)
        {
            foreach (Equipment item in equipment)
            {
                Equipment.Add(item.Config);
            }
            foreach (Item inventoryItem in inventory)
            {
                Inventory.Add(inventoryItem.Config);
            }
        }
    }


}