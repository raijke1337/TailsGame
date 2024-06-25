using Arcatech.Items;
using Arcatech.Triggers;
using KBCore.Refs;
using System;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.UI.GridLayoutGroup;

namespace Arcatech.Units
{
    public class UnitInventoryComponent : ManagedControllerBase
    {
        private BaseUnit _owner;
        private Dictionary<EquipmentType, EquipmentItem> _equips;
        private List<InventoryItem> _items;
        private List<InventoryItem> _drops;

        private ItemEmpties _empties;

        public IReadOnlyCollection<EquipmentItem> GetCurrentEquips => _equips.Values;
        public IReadOnlyCollection<InventoryItem> GetCurrentInventory => _items;

        public event SimpleEventsHandler<UnitInventoryComponent> InventoryUpdateEvent;
        protected void CallBackUpdateEvent() => InventoryUpdateEvent?.Invoke(this);

        #region used by level events 

        public bool HasItem(Item search)
        {
            return (_equips.Values.Any(t=>t.ID==search.ID) || _items.Any(t=>t.ID==search.ID));
        }
        public void PickedUpItem(Item i,bool wantEquip)
        {
            if (_items.Where(t => t.ID == i.ID).Any() || _equips.Where(t => t.Value.ID == i.ID).Any())
            {
               // Debug.Log($"Not added item {i} to {_owner} because it is already owned");
                return; // supposed to prevent duplication 
            }
            else
            {
                var res = ProduceItem(i);
                //_items.Add(res);

                if (wantEquip && res is EquipmentItem e)
                {
                    MoveToEquipped(e);
                }
                else
                {
                    MoveToInventory(res);
                }
                //Debug.Log($"Added item {i} to {_owner}");
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
                    MoveToEquipped(eq);
                }
            }
        }
        public void MoveToSheathed(EquipmentItem i)
        {

        }
        private void MoveToEquipped(EquipmentItem i)
        {
            if (_equips.TryGetValue(i.ItemType, out var equippedAlready))
            {
                MoveToInventory(equippedAlready);
            }
            _items.Remove(i);
            _equips[i.ItemType] = i;

            //i.OnEquip(_empties.ItemPositions[i.ItemPlaceType]);
        }
        private void MoveToInventory(InventoryItem i)
        {
            if (_equips.ContainsKey(i.ItemType))
            {
                _equips.Remove(i.ItemType);
            }
            _items.Add(i);
            if (i is EquipmentItem eq)
            {
                eq.OnUnequip();
            }
        }
        // to choose the text for the context button

        #endregion


        public bool IsItemEquipped (EquipmentType type, out EquipmentItem item)
        {
            return _equips.TryGetValue(type, out item);
        }

        public UnitInventoryComponent DrawItems (IDrawItemStrategy strategy)
        {
            foreach (var e in _equips.Values)
            {
                e.SetItemEmpty(_empties.ItemPositions[strategy.GetPlaces[e.ItemType]]);
            }
            return this;
        }

        #region setup
        public SerializedStatModConfig[] GetCurrentMods
        {
            get
            {
                var list = new List<SerializedStatModConfig>();
                foreach (var equip in _equips.Values)
                {
                    if (equip.ItemStats != null)
                    {
                        list.AddRange(equip.ItemStats);
                    }
                }

                return list.ToArray();

            }
        }


        public UnitInventoryComponent LoadSerializedItems (UnitInventoryItemConfigsContainer cfg, BaseUnit owner)
        {
            _owner = owner;

            _equips = new Dictionary<EquipmentType, EquipmentItem>();
            _items = new List<InventoryItem>();

            foreach (var e in cfg.Equipment)
            {
                _equips[e.ItemType] = ProduceItem(e) as EquipmentItem;
            }
            foreach (var i in cfg.Inventory)
            {
                _items.Add(ProduceItem(i));
            }

            return this;
        }
        public UnitInventoryComponent SetItemEmpties(ItemEmpties e)
        {
            _empties = e;
            return this;
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


        #region managed
        public override void StartController()
        {
            base.StartController();
        }
        public override void UpdateController(float delta)
        {

        }

        public override void StopController()
        {

        }
        #endregion
        #endregion
    }


}