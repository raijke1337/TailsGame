using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CursorManager
{
    private CursorType _currentType;
    [SerializeField] private Texture2D _texture;

    private Dictionary<CursorType, Texture2D> _shapes = new Dictionary<CursorType, Texture2D>();
    public void SetCursor(CursorType type)
    {
        _currentType = type;
        _texture = _shapes[_currentType];
        Cursor.SetCursor(_shapes[type], Vector2.zero, CursorMode.Auto);
    }

    public void ResetCursor()
    {
        SetCursor(CursorType.Menu);
        Cursor.visible = true;
    }

    public CursorManager(CursorsDictionary dict)
    {
        foreach (var key in dict.GetCursors.Keys)
        {
            _shapes[key] = dict.GetCursors[key];
        }
        ResetCursor();

    }
    public CursorManager(Dictionary<CursorType, Texture2D> shapes)
    {
        foreach (var key in shapes.Keys)
        {
            _shapes[key] = shapes[key];
        }
        ResetCursor();
    }

}

