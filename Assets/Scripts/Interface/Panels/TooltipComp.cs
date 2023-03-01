using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class TooltipComp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _desc;
    private RectTransform _rect;

    public RectTransform GetRect { get => _rect; }

    public void SetTexts(ItemContent content)
    {
        if (_name == null | _desc == null)
        {
            Debug.LogWarning("Set fields for " + this);
            return;
        }
        if (content == null) return;
        _name.text = content.DisplayName;
        TextContainer descC;
        try
        {
            descC = (TextsManager.Instance as TextsManager).GetContainerByID(content.ID);
        }
        catch
        {
            Debug.LogWarning($"No text description for {content.ID}");
        }
        var result = string.Join(
            content.ItemType.ToString(), "\n",
            content.SkillString, "\n"
            );
        
        _desc.text = result;

    }
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }



}
