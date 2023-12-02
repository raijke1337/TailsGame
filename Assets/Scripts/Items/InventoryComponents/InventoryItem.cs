using Arcatech.Managers;
using Arcatech.Units;
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
        public string GetDisplayName => _description.GetTitle;
        public string GetDescription => _description.GetFormattedText;

        public EquipItemType ItemType { get; }
        protected TextContainer _description;
        public BaseUnit Owner;


        public InventoryItem(Item config, BaseUnit ow)
        {
            ID = config.ID; ItemIcon = config.ItemIcon; ItemType = config.ItemType;
            _description = TextsManager.Instance.GetContainerByID(ID);
            Owner= ow;
        }
        

    }
}