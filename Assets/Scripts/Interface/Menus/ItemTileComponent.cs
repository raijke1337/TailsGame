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
    private Image _imageComp;

    [SerializeField] private Sprite DefaultImg;
    [SerializeField] private ItemContent _content;

    public InterfaceTilesEvent ClickEvent;
    public InterfaceTilesEvent HoverEvent;
    public InterfaceTilesEvent UnhoverEvent;


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
        ClickEvent?.Invoke(this,eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverEvent?.Invoke(this,eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnhoverEvent?.Invoke(this, eventData);   
    }
    private void OnItemSet(ItemContent content)
    {
        if (content == null) _imageComp.sprite = DefaultImg;
        else  _imageComp.sprite = content.ItemIcon;
    }
}

