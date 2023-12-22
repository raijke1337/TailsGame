using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    // coin, upgrade, key etc... NYI
    public class InventoryItem
    {
        public string ID;
        public Sprite ItemIcon { get => _description.Picture; }
        public string GetDisplayName => _description.Title;
        public string GetDescription => _description.GetFormattedText;

        public EquipItemType ItemType { get; }
        protected TextContainerSO _description;
        public BaseUnit Owner;


        public InventoryItem(Item config, BaseUnit ow)
        {
            ID = config.ID; ItemType = config.ItemType;
            _description = (config.Description);
            Owner = ow;
        }


    }
}