using System;
using Arcatech.Items;
using AYellowpaper.SerializedCollections;

namespace Arcatech.Units
{
    [Serializable]

    public class UnitInventoryComponent
    {
        public SerializedDictionary<EquipItemType, EquipmentItem> Equips;
        public SerializedDictionary<EquipItemType, InventoryItem> Inventory;



    }
}