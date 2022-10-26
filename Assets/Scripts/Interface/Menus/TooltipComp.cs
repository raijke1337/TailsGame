using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipComp : MonoBehaviour
{
    private Text _text;
    public ItemContent Content { get; set; }
    private RectTransform _rect;
    private void Awake()
    {
        _text = GetComponentInChildren<Text>();
        _rect = GetComponentInChildren<RectTransform>();
    }

    public void UpdateLocation(Vector2 cursorLoc)
    {
        _rect.position = cursorLoc;
        _text.text = Content.DisplayName;
    }

}
