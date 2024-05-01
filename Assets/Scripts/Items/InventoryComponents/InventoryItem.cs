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
        public ExtendedTextContainerSO GetDescription => _description;

        public EquipItemType ItemType { get; }
        protected ExtendedTextContainerSO _description;
        public BaseUnit Owner;


        public InventoryItem(Item config, BaseUnit ow)
        {
            ID = config.ID; ItemType = config.ItemType;
            _description = (config.Description);
            Owner = ow;
        }


    }
}