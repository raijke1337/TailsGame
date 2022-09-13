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

public class MenuPanel : MonoBehaviour, IHasID
{
    [SerializeField] protected string _id;
    public string GetID => _id;

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


    #region unity
    public void OtherMenuButtonClicked(string menu) => SwitchToWindow?.Invoke(menu,this);

    #endregion

}

