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

public abstract class BaseStats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Name;

    public int ID;
    // todo load stuff by ID from somewhere

    public virtual BaseStats GetData() => this;

    public event FocusEventHandler OnFocusEventHandler;


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        MouseOverEvent(this, true);
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        MouseOverEvent(this, false);
    }
    protected void MouseOverEvent(BaseStats stats, bool isSelected)
    {
        OnFocusEventHandler?.Invoke(stats, isSelected);
    }

}

