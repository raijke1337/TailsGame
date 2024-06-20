using Arcatech.Texts;
using Arcatech.Units;
using System;
using UnityEngine;

namespace Arcatech.Items
{
    // coin, upgrade, key etc... NYI
    [Serializable]
    public class InventoryItem
    {
        public string ID;
        public Sprite ItemIcon { get => _description.Picture; }
        public ExtendedText GetDescription => _description;

        public EquipmentType ItemType { get; }
        protected ExtendedText _description;
        public BaseUnit Owner;


        public InventoryItem(Item config, BaseUnit ow)
        {
            ID = config.ID; ItemType = config.ItemType;
            _description = (config.Description);
            Owner = ow;
        }


    }
}