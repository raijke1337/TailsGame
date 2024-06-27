using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Skills;
using Arcatech.Triggers;
using KBCore.Refs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.UI.GridLayoutGroup;

namespace Arcatech.Units
{
    public class UnitInventoryComponent : ManagedControllerBase
    {
        private Dictionary<EquipmentType, EquipmentItem> _equips;
        private List<InventoryItem> _items;
        private List<InventoryItem> _drops;

        private ItemEmpties _empties;

        public IReadOnlyCollection<EquipmentItem> GetCurrentEquips => _equips.Values;
        public IReadOnlyCollection<InventoryItem> GetCurrentInventory => _items;

        

        #region used by level events  and UI

        public void OnplayerPickedUpItem(Item i, bool wantEquip)
        {
            if (_items.Where(t => t.ID == i.ID).Any() || _equips.Where(t => t.Value.ID == i.ID).Any())
            {
                return; // supposed to prevent duplication 
            }
            else
            {
                var res = ProduceItem(i);
                _items.Add(res);

                if (wantEquip && res is EquipmentItem e)
                {
                    MoveToEquipped(e);
                }
                else
                {
                    MoveToInventory(res);
                }
            }
        }

        private InventoryItem ProduceItem(Item i)
        {
            return new InventoryItem(i);
        }

        public void HandleSwapButton(InventoryItem i)
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
        }
        // to choose the text for the context button

        #endregion

        #region checks
        public bool IsItemEquipped(EquipmentType type, out EquipmentItem item)
        {
            return _equips.TryGetValue(type, out item);
        }
        public bool HasItem<T>(T type, out InventoryItem item) where T : Item
        {
            item = _items.FirstOrDefault(t => t.ID == type.ID);
            if (item == null) return false;
            return true;
        }
        public bool HasItem(string ID, out InventoryItem item)
        {
            item = _items.FirstOrDefault(t => t.ID == ID);
            if (item == null)
            {
                item = _equips.Values.FirstOrDefault(t => t.ID == ID);
                if (item == null) { return false; }
                return true;
            }
            return true;
        }
        #endregion


        public UnitInventoryComponent DrawItems(IDrawItemStrategy strategy)
        {
            foreach (var e in _equips.Values)
            {
                ItemPlaceType placeType = strategy.GetPlaces[e.ItemType];
                switch (placeType)
                {
                    case ItemPlaceType.Hidden:
                        e.ItemShown = false;
                        break;
                    default:
                        e.SetItemEmpty(_empties.ItemPositions[strategy.GetPlaces[e.ItemType]]);
                        break;
                }
            }
            return this;
        }

        public SerializedSkillConfiguration[] GetSkillConfigs
        {
            get
            {
                List<SerializedSkillConfiguration> foundSkills = new();
                foreach (var e in _equips.Values)
                {
                    if (e is IHasSkill ss)
                    {
                        foundSkills.Add(ss.GetSkillData);
                    }
                }
                return foundSkills.ToArray();
            }
        } 

        public IWeapon[] GetWeapons
        {
            get
            {
                List<IWeapon> weaps = new();
                foreach (var e in _equips.Values)
                {
                    if (e is IWeapon ww)
                    {
                        weaps.Add(ww);
                        
                    }
                }
                return weaps.ToArray();
            }
        }

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



        #region setup


        public UnitInventoryComponent LoadSerializedItems (UnitInventoryItemConfigsContainer cfg, BaseUnit owner)
        {
            _equips = new Dictionary<EquipmentType, EquipmentItem>();
            _items = new List<InventoryItem>();

            foreach (var e in cfg.Equipment)
            {
                if (e is IWeapon)
                {
                    _equips[e.ItemType] = ProduceItem(e) as WeaponItem;
                }
                else
                {
                    _equips[e.ItemType] = ProduceItem(e) as EquipmentItem;
                }

            }
            foreach (Item i in cfg.Inventory)
            {
                _items.Add(ProduceItem(i));
            }

            EventBus<InventoryUpdateEvent>.Raise(new InventoryUpdateEvent(Owner, this));

            return this;
        }

        public UnitInventoryComponent SetItemEmpties(ItemEmpties e)
        {
            _empties = e;
            return this;
        }





        #region managed  - UNUSED
        public override void StartController()
        {

        }
        public override void ControllerUpdate(float delta)
        {

        }

        public override void StopController()
        {
        }

        public override void FixedControllerUpdate(float fixedDelta)
        {
        }
        #endregion
        #endregion
    }


}