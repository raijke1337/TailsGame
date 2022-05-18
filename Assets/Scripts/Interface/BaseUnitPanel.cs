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

public abstract class BaseUnitPanel : MonoBehaviour
{
    
    private BaseUnit baseUnit;
    [SerializeField] protected float _barFillRateMult = 1f;


    protected readonly Color minColorDefault = Color.red;
    protected readonly Color maxColorDefault = Color.black;



    public virtual void AssignItem(BaseUnit item, bool isSelect)
    {
        baseUnit = item;
        baseUnit.ToggleCamera(isSelect);
        gameObject.SetActive(isSelect);
        RunSetup();
    }


    protected virtual void ColorTexts(Text text, float max, float current, Color minC, Color maxC )
    {
        text.color = Color.Lerp(minC, maxC, current / max);
    }


    protected virtual void LateUpdate()
    {
        if (baseUnit == null) return;
        UpdatePanel();
    }

    protected abstract void UpdatePanel();

    protected abstract void RunSetup();

    protected virtual void Start()
    {
        if (baseUnit == null) gameObject.SetActive(false);
    }




}

