using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Items
{
    // coin, upgrade, key etc... NYI
    public class InventoryItem
    {
        public string ID;
        public Sprite ItemIcon;
        public string DisplayName;
        public EquipItemType ItemType;
        public TextContainer Description;

        public InventoryItem(Item config)
        {
            ID = config.ID; ItemIcon = config.ItemIcon; ItemType = config.ItemType;
            DisplayName = config.DisplayName; Description = TextsManager.Instance.GetContainerByID(ID);
        }
        

    }
}