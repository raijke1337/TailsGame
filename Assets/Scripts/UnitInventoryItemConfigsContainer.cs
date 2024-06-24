using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Managers.Save;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Items
{

    #region items

    // used for npc inventory init
    [Serializable]
    public class UnitInventoryItemConfigsContainer
    {
        public string ID;
        [SerializeField] public List<Equip> Equipment;
        [SerializeField, Space] public List<Item> Inventory;
        public UnitInventoryItemConfigsContainer(UnitItemsSO cfg)
        {
            LoadCfg(cfg);
        }
        // used for player inventory load
        public UnitInventoryItemConfigsContainer(SerializedUnitInventory inv)
        {
            Equipment = new List<Equip>();
            Inventory = new List<Item>();

            foreach (string id in inv.Inventory)
            {
                var i = DataManager.Instance.GetConfigByID<Item>(id);
                if (i is Equip e)
                {
                    Inventory.Add(e);
                }
                else
                {
                    Inventory.Add(i);
                }
            }
            foreach (string id in inv.Equips)
            {
                var i = DataManager.Instance.GetConfigByID<Item>(id);
                if (i is Equip e)
                {
                    Equipment.Add(e);
                }
            }
        }
        // fpr saving
        public UnitInventoryItemConfigsContainer(UnitInventoryComponent cfg)
        {
            Equipment = new List<Equip>();
            Inventory = new List<Item>();

            foreach (var item in cfg.GetCurrentInventory)
            {
                var i = DataManager.Instance.GetConfigByID<Item>(item.ID);
                if (i is Equip e)
                {
                    Inventory.Add(e);
                }
                else
                {
                    Inventory.Add(i);
                }
            }
            foreach (var item in cfg.GetCurrentEquips)
            {
                var i = DataManager.Instance.GetConfigByID<Item>(item.ID);
                if (i is Equip e)
                {
                    Equipment.Add(e);
                }
                else
                {
                    Debug.Log($"Trying to save {item} as equipped and failed");
                }
            }


        }
        public UnitInventoryItemConfigsContainer()
        {

        }
        public void LoadCfg (UnitItemsSO cfg)
        {
            Equipment = new List<Equip>(cfg.Equipment);
            Inventory = new List<Item>();

            foreach (Item i in cfg.Inventory)
            {
                if (i is Equip e)
                {
                    Inventory.Add(e);
                }
                else
                {
                    Inventory.Add(i);
                }
            }
            ID = cfg.ID;
        }

    }

    #endregion



}