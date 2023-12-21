using Arcatech.Units;
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
            ID = config.ID; ItemIcon = config.Description.Picture; ItemType = config.ItemType;
            _description = new TextContainer(config.Description);
            Owner = ow;
        }


    }
}