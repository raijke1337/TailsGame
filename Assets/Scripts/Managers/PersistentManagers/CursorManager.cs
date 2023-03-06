using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CursorManager : MonoBehaviour
{
    #region SingletonLogic

    public static CursorManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    #endregion


    [SerializeField] Texture2D currentShape;
    [SerializeField] string CursorsSetName = "default";

    private Dictionary<CursorType, Texture2D> _shapes;

    private void Start()
    {
        var dict = DataManager.Instance.GetConfigByID<CursorsDictionary>(CursorsSetName);
        _shapes = new Dictionary<CursorType, Texture2D>();
        foreach (var key in dict.GetCursors.Keys)
        {
            _shapes[key] = dict.GetCursors[key];
        }
        ResetCursor();
    }
    public void SetCursor(CursorType type)
    {
        if (_shapes == null) return;
        currentShape = _shapes[type];
        Cursor.SetCursor(_shapes[type], Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
    }

    public void ResetCursor()
    {
        SetCursor(CursorType.Menu);
        Cursor.visible = true;
    }


}

