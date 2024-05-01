using Arcatech.Items;
using Arcatech.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace Arcatech.Units
{
    [Serializable]
    public class UnitInventoryComponent
    {
        private BaseUnit _owner;
        private Dictionary<EquipItemType, EquipmentItem> _equips;
        private List<InventoryItem> _items;
        private List<InventoryItem> _drops;
        
        public SerializedUnitInventory PackSaveData // used by game manager to save data
        {
            get
            {

                SerializedUnitInventory cont = new SerializedUnitInventory();
                foreach (var e in _equips.Values)
                {
                    cont.Equips.Add(e.ID);
                }
                foreach (var e in _items)
                {
                    cont.Inventory.Add(e.ID);
                }
                foreach (var d in _drops)
                {
                    // NYI player should not drop anything
                }

                return cont;
            }
        }

        public IReadOnlyCollection<EquipmentItem> GetCurrentEquips => _equips.Values;
        public IReadOnlyCollection<InventoryItem> GetCurrentInventory => _items;

        public event SimpleEventsHandler<UnitInventoryComponent,SerializedUnitInventory> InventoryUpdateEvent;
        protected void CallBackUpdateEvent() => InventoryUpdateEvent?.Invoke(this, PackSaveData);

        #region used by level events 

        public bool HasItem(Item search)
        {
            return (_equips.Values.Any(t=>t.ID==search.ID) || _items.Any(t=>t.ID==search.ID));
        }
        public void PickedUpItem(Item i)
        {
            if (_items.Where(t => t.ID == i.ID).Any() || _equips.Where(t => t.Value.ID == i.ID).Any())
            {
                Debug.LogWarning($"Not added item {i} to {_owner} because it is already owned");
                return; // supposed to prevent duplication 
            }
            else
            {
                var res = ProduceItem(i);
                _items.Add(res);
                CallBackUpdateEvent();
            }
        }

        #endregion

        #region equipments level

        public void HandleSwapButton (InventoryItem i)
        {
            if (i is EquipmentItem eq)
            {
                if (GetCurrentEquips.Contains(eq))
                {
                    MoveToInventory(eq);
                }
                else
                {
                    MoveToEquips(eq);
                }
            }
        }

        private void MoveToEquips(EquipmentItem i)
        {
            _items.Remove(i);
            _equips[i.ItemType] = i;
        }
        private void MoveToInventory(EquipmentItem i)
        {
            _equips.Remove(i.ItemType);
            _items.Add(i);
        }
        // to choose the text for the context button
        public bool IsItemEquipped(EquipmentItem equip)
        {
            return (_equips.TryGetValue(equip.ItemType, out var eq));
        }

        #endregion



        // for default inventory setup from config SOs
        public UnitInventoryComponent(UnitInventoryItemConfigsContainer cfg, BaseUnit owner)
        {
            _owner = owner;

            _equips = new Dictionary<EquipItemType, EquipmentItem>();
            _items = new List<InventoryItem>();
            _drops = new List<InventoryItem>();

            foreach (var e in cfg.Equipment)
            {
                _equips[e.ItemType] = ProduceItem(e) as EquipmentItem;
            }
            foreach (var i in cfg.Inventory)
            {
                _items.Add(ProduceItem(i));
            }
            foreach (var i in cfg.Drops)
            {
                _drops.Add(ProduceItem(i));
            }
        }

        private InventoryItem ProduceItem(Item cfg)
        {
            InventoryItem ret;
            if (cfg is Equip e)
            {
                ret = new EquipmentItem(e, _owner);

                if (cfg is Weapon w)
                {
                    if (cfg is RangedWeapon r)
                    {
                        ret = new RangedWeaponItem(r, _owner);
                    }
                    else
                    {
                        ret = new WeaponItem(w, _owner);
                    }
                }
                if (cfg is Booster b)
                {
                    ret = new BoosterItem(b,_owner);
                }
            }
            else
            {
                ret =  new InventoryItem(cfg, _owner);
            }

            //Debug.Log($"{ret.ID} config as {ret.GetType()}");

            return ret;
            //cringe but it should work
        }


        // for player laod from save
        public UnitInventoryComponent(SerializedUnitInventory strings, BaseUnit owner)
        {
            _owner = owner;

            _equips = new Dictionary<EquipItemType, EquipmentItem>();
            _items = new List<InventoryItem>();
            _drops = new List<InventoryItem>();

            foreach (var e in strings.Equips)
            {
                var item = DataManager.Instance.GetConfigByID<Equip>(e);
                _equips[item.ItemType] = ProduceItem(item) as EquipmentItem;
            }

            foreach (var i in strings.Inventory)
            {
                var item = DataManager.Instance.GetConfigByID<Item>(i);
                _items.Add(ProduceItem(item));
            }
        }


    }
}