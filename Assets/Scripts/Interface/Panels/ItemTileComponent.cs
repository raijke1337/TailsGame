using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTileComponent : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public event SimpleEventsHandler<ItemContent> ItemClickedEvent;
    public event SimpleEventsHandler<ItemContent, bool> ItemTooltipEvent;

    private Image _imageComp;


    [SerializeField] private Sprite DefaultImg;
    [SerializeField] private ItemContent _content;


    public ItemContent Content
    {
        get
        { return _content; }
        set
        {
            _content = value;
            OnItemSet(_content);
        }
    }
    public void Clear()
    {
        Content = null;
        OnItemSet(null);
    }

    private void Awake()
    {
        _imageComp = GetComponent<Image>();
        _imageComp.sprite = DefaultImg;
    }

    public RectTransform GetRekt => GetComponent<RectTransform>();

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemClickedEvent?.Invoke(Content);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemTooltipEvent?.Invoke(Content, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltipEvent?.Invoke(Content, false);
    }
    private void OnItemSet(ItemContent content)
    {
        if (content == null) _imageComp.sprite = DefaultImg;
        else _imageComp.sprite = content.ItemIcon;
    }
}

