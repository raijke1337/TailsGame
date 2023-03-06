using System;
using System.Linq;
using UnityEngine;

public class MenuPanelTiled : MenuPanel
{
    private RectTransform currentArea;
    [SerializeField, Tooltip("offset between internal elements")] private Vector2 _offsets;
    [SerializeField, Tooltip("internal element size")] private Vector2 _tileSize;

    public event SimpleEventsHandler<ItemContent, bool> ShowtooltipToggle;
    public event SimpleEventsHandler<ItemContent> TileClickToggle;
    protected void TooltipCallback(ItemContent cont, bool isShow)
    {
        //Debug.Log($"Tooltip callback for {this}, item ID {cont.ID}, showing: {isShow}");
        if (cont == null) return; ShowtooltipToggle?.Invoke(cont, isShow);
    }
    protected void TileClickCallback(ItemContent cont)
    {
        //Debug.Log($"Tile click callback for {this}, item ID {cont.ID}");
        if (cont == null) return; TileClickToggle?.Invoke(cont);
    }



    protected Vector2[] _elementsLoc;
    [SerializeField] protected ItemTileComponent TilePrefab;
    protected ItemTileComponent[] _tiles;


    protected void SubscribeToTileEvents(ItemTileComponent tile, bool isEnable)
    {
        if (isEnable)
        {
            tile.ItemTooltipEvent += TooltipCallback;
            tile.ItemClickedEvent += TileClickCallback;
        }
    }


    public virtual void CreateTilesAndSub(int total)
    {
        GetLocationsForTiles();
        _tiles = new ItemTileComponent[_elementsLoc.Length];

        int number = total;
        if (number > _elementsLoc.Length)
        {
            number = _elementsLoc.Length;
        }

        for (int i = 0; i < number; i++)
        {
            _tiles[i] = Instantiate(TilePrefab);
            _tiles[i].GetRekt.SetParent(transform);
            _tiles[i].GetRekt.anchoredPosition = _elementsLoc[i];
            SubscribeToTileEvents(_tiles[i], true);
        }

    }
    protected void GetLocationsForTiles()
    {
        if (currentArea != null) return;

        currentArea = GetComponent<RectTransform>();
        _elementsLoc = Extensions.GetTilePositions(currentArea, _offsets, _tileSize);

    }


    public virtual void AddTileContent(ItemContent content)
    {
        if (content == null) return;
        try
        {
            var tile = _tiles.First(t => t.Content == null);
            tile.Content = content;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"No free space for item in {this} {e}");
        }
    }
    public virtual void RemoveTileContent(ItemContent content)
    {
        if (content == null) return;
        try
        {
            var tile = _tiles.First(t => t.Content == content);
            tile.Clear();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"No free space for item in {this} {e}");
        }
    }

    protected virtual void OnDisable()
    {
        if (_tiles == null) return;
        foreach (var tile in _tiles)
        {
            if (tile == null) return;
            SubscribeToTileEvents(tile, false);
        }
    }


}
