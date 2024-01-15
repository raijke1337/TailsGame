using Arcatech.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Arcatech.UI
{
    public class ItemTileComponent : IconTileComp
    {
        public event SimpleEventsHandler<InventoryItem> ItemClickedEvent;
        public event SimpleEventsHandler<InventoryItem, bool> ItemTooltipEvent; 
        private InventoryItem _item;
        public InventoryItem Item
        {
            get
            { return _item; }
            set
            {
                _item = value;
                SetItemIcon(_item);
            }
        }


        private void SetItemIcon(InventoryItem content)
        {
            if (content == null) Clear();
            else SetSprite(content.ItemIcon);
        }


        public override void Clear()
        {
            Item = null;
            base.Clear();
        }


        protected override void CallbackClick(PointerEventData data)
        {

            ItemClickedEvent?.Invoke(Item);
        }
        protected override void CallbackEnter(PointerEventData data)
        {
            ItemTooltipEvent?.Invoke(Item, true);
        }
        protected override void CallbackExit(PointerEventData data)
        {
            ItemTooltipEvent?.Invoke(Item, false);
        }

    }

}