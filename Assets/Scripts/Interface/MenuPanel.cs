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

public abstract class MenuPanel : MonoBehaviour
{
    public string PanelID;

    protected Coroutine scalerCor;
    private Vector2 _openSize;
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _openSize = _rect.sizeDelta;
    }

    public virtual void OnToggle(string ID)
    {
        if (ID == PanelID)
        {
            bool needToHide = _openSize == _rect.sizeDelta;
            scalerCor = StartCoroutine(LerpScale(needToHide));
        }         
    }

    protected IEnumerator LerpScale(bool isHide)
    {
        yield return null;
    }

}

