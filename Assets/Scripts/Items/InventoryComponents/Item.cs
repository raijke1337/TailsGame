using Arcatech.Texts;
using Arcatech.Units;
using System;
using UnityEngine;

namespace Arcatech.Items
{
    // coin, upgrade, key etc..
    [Serializable]
    public class Item : IItem
    {
        public DummyUnit Owner { get; }
        public SerializableGuid ID;
        public ItemSO Config;
        public Item(ItemSO cfg, DummyUnit ow)
        {
            ID = SerializableGuid.NewGuid();
            Owner = ow;
            Config = cfg;
            Type = cfg.Type;
        }


        public EquipmentType Type { get; protected set; }
    }
}