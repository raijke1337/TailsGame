using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Arcatech.UI
{
    public class IconTileComp :MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        #region ipointer
        public void OnPointerClick(PointerEventData eventData) => CallbackClick(eventData);

        public void OnPointerEnter(PointerEventData eventData) => CallbackEnter(eventData);
        public void OnPointerExit(PointerEventData eventData) => CallbackExit(eventData);
        #endregion

        [SerializeField] protected Image _itemImg;
        protected Sprite _defaultImg;
        public void SetSprite(Sprite s)
        {
            _defaultImg = _itemImg.sprite;
            if (s!=null) _itemImg.sprite = s;
        }


        public virtual void Clear()
        {
            _itemImg.sprite = _defaultImg;
        }


        public event SimpleEventsHandler<IconTileComp> ClickedEvent;

        protected virtual void CallbackClick(PointerEventData data)
        {
            ClickedEvent?.Invoke(this);
        }
        protected virtual void CallbackEnter(PointerEventData data)
        {

        }
        protected virtual void CallbackExit(PointerEventData data)
        {

        }



    }
}