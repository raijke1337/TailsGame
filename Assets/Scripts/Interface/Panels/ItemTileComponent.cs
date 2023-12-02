using Arcatech.Items;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Arcatech.UI
{
    public class ItemTileComponent : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public event SimpleEventsHandler<InventoryItem> ItemClickedEvent;
        public event SimpleEventsHandler<InventoryItem, bool> ItemTooltipEvent;

        [SerializeField] private Image _itemImg;
        [SerializeField] private Sprite DefaultImg;
        [SerializeField] private InventoryItem _item;


        public InventoryItem Item
        {
            get
            { return _item; }
            set
            {
                _item = value;
                OnItemSet(_item);
            }
        }
        public void Clear()
        {
            Item = null;
            OnItemSet(null);
        }


        public RectTransform GetRekt => GetComponent<RectTransform>();

        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("Clicked");
            ItemClickedEvent?.Invoke(Item);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ItemTooltipEvent?.Invoke(Item, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemTooltipEvent?.Invoke(Item, false);
        }
        private void OnItemSet(InventoryItem content)
        {
            if (content == null) _itemImg.sprite = DefaultImg;
            else _itemImg.sprite = content.ItemIcon;
        }
    }

}