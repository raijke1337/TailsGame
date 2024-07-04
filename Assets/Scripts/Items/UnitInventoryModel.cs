using Arcatech.Managers;
using Arcatech.Units;
using System;
using System.Collections.Generic;

namespace Arcatech.Items
{
    public class UnitInventoryModel
    {
        public DummyUnit Owner {get;}
        public ObservableArray<Item> Inventory { get; protected set; }
        public ObservableDictionary<EquipmentType, Equipment> Equipments { get; protected set; }


        public event Action<IEnumerable<Item>> OnInventoryChange
        {
            add => Inventory.AnyRecordChanged += value;
            remove => Inventory.AnyRecordChanged -= value;
        }
        public event Action<IEnumerable<Equipment>> OnEquipsChange
        {
            add => Equipments.AnyValueChanged += value;
            remove => Equipments.AnyValueChanged -= value;
        }


        public UnitInventoryModel(UnitInventoryItemConfigsContainer cfgs, DummyUnit owner)
        {
            Owner = owner;  
            Inventory = new ObservableArray<Item>();
            foreach (ItemSO item in cfgs.Inventory)
            {
                Inventory.TryAdd(DataManager.Instance.ItemsFactory.ProduceItem(item, Owner) as Item);
            }

            List<KeyValuePair<EquipmentType, Equipment>> list = new();

            foreach (EquipSO e in cfgs.Equipment)
            {
                var eq = DataManager.Instance.ItemsFactory.ProduceItem(e, Owner);

                list.Add(new KeyValuePair<EquipmentType, Equipment>(eq.Type, eq as Equipment));

            }

            Equipments = new ObservableDictionary<EquipmentType, Equipment>(list.ToArray());
        }

        public Item PickItem(int index) => Inventory[index] as Item;
        public void Clear() => Inventory.Clear();
        public bool AddItem(Item i) => Inventory.TryAdd(i);
        public bool RemoveItem(Item i) => Inventory.TryRemove(i);

    }


}