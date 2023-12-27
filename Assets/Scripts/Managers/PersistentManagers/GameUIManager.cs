using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class GameUIManager : MonoBehaviour
{
    #region SingletonLogic

    public static GameUIManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    #endregion

    [SerializeField] private SerializedDictionary<CursorType, Texture2D> _cursors;
    [SerializeField] private SerializedDictionary<FontType, TMP_FontAsset> _fonts;

#if UNITY_EDITOR
    private void OnEnable()
    {
        Assert.IsNotNull(_cursors);
        Assert.IsTrue(_cursors.Count > 0);
        Assert.IsNotNull(_fonts);
        Assert.IsTrue(_fonts.Count > 0);
    }
#endif
    private void Start()
    {
        SetCursor(CursorType.Menu);
        Cursor.visible = true;
    }
    public void SetCursor(CursorType type)
    {
        Cursor.SetCursor(_cursors[type], Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
    }
    public TMP_FontAsset GetFont(FontType t) => _fonts[t];
   

}

