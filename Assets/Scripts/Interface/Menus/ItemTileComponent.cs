using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemTileComponent : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public event SimpleEventsHandler<ItemContent> ItemClickedEvent;

    private Image _imageComp;

    [SerializeField] private Sprite DefaultImg;
    [SerializeField] private ItemContent _content;

    [SerializeField] private RectTransform _textPanel;
    private RectTransform _instantiatedTooltip;

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
        if (_content == null) return;
        var pos = eventData.position;
        _instantiatedTooltip = Instantiate(_textPanel, pos, Quaternion.identity);
        var txt = _instantiatedTooltip.GetComponentInChildren<Text>();
        txt.text = _content.DisplayName;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_instantiatedTooltip != null) Destroy(_instantiatedTooltip.gameObject);
    }
    private void OnItemSet(ItemContent content)
    {
        if (content == null) _imageComp.sprite = DefaultImg;
        else  _imageComp.sprite = content.ItemIcon;
    }
}

