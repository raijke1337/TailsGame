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

    private TooltipComp tooltipComp;

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

        try
        {
            tooltipComp = Instantiate(Extensions.GetAssetsOfType<TooltipComp>(Constants.PrefabsPaths.c_InterfacePrefabs).First(t => t.name == "ItemText"));
            tooltipComp.transform.parent = transform;
            tooltipComp.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log($"Mising tooltip prefab");
        }

    }

    public RectTransform GetRekt => GetComponent<RectTransform>();

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemClickedEvent?.Invoke(Content);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_content == null) return;

        tooltipComp.Content = _content;
        tooltipComp.gameObject.SetActive(true);
        tooltipComp.UpdateLocation(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipComp.gameObject.SetActive(false);
    }
    private void OnItemSet(ItemContent content)
    {
        if (content == null) _imageComp.sprite = DefaultImg;
        else  _imageComp.sprite = content.ItemIcon;
    }
}

