using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuPanel : MonoBehaviour, IHasID
{
    [SerializeField] protected string _id;
    public string GetID => _id;
    public override string ToString()
    {
        return _id;
    }

    public bool IsOpen { get; protected set; } = false;
    public bool StartClosed = false;
    public event SimpleEventsHandler<string,MenuPanel> SwitchToWindow;

    protected Vector2[] _elementsLoc;
    [SerializeField] protected ItemTileComponent TilePrefab;
    protected ItemTileComponent[] _tiles;


    protected virtual void Awake()
    {
        gameObject.name = _id;
    }

    public virtual void OnToggle(bool isShow)
    {
        gameObject.SetActive(isShow);
        IsOpen = isShow;
    }

    private RectTransform currentArea;
    [SerializeField, Tooltip("offset between internal elements")] private Vector2 _offsets;
    [SerializeField, Tooltip("internal element size")] private Vector2 _tileSize;


    protected virtual void Start()
    {
        currentArea = GetComponent<RectTransform>();
        _elementsLoc = Extensions.GetTilePositions(currentArea, _offsets, _tileSize);

        _tiles = new ItemTileComponent[_elementsLoc.Length];
        gameObject.SetActive(!StartClosed);
    }

    protected void SubscribeToTiles(bool isStart)
    {
        foreach (var t in _tiles)
        {
            if (t == null) break;
            if (isStart)
            {
                t.ClickEvent += OnTileClicked;
                t.HoverEvent += OnTileHovered;
                t.UnhoverEvent += OnTileUnhovered;
            }
            else
            {
                t.ClickEvent -= OnTileClicked;
                t.HoverEvent -= OnTileHovered;
                t.UnhoverEvent -= OnTileUnhovered;
            }
        }
    }

    private void OnDisable()
    {
        SubscribeToTiles(false);
    }

    #region interactions
    public void OtherMenuButtonClicked(string menu) => SwitchToWindow?.Invoke(menu, this);
    protected virtual void OnTileClicked(ItemTileComponent tile, PointerEventData data) 
    {Debug.Log($"{tile} was clicked in {GetID}, item: {tile.Content}, place {data.pressPosition}");}
    protected virtual void OnTileHovered(ItemTileComponent tile, PointerEventData data)
    { Debug.Log($"{tile} is hovered in {GetID}, item: {tile.Content}"); }

    protected virtual void OnTileUnhovered(ItemTileComponent tile, PointerEventData data)
    { Debug.Log($"{tile} is no longer hovered in {GetID}, item: {tile.Content}"); }


    #endregion

}

