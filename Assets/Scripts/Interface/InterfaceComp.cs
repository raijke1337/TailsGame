using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterfaceComp : MonoBehaviour
{
    public bool IsVisible { get; private set; }
    public bool IsInAnimation { get; set; }

    public virtual void ToggleActiveState()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        IsVisible = gameObject.activeSelf;
    }

}
