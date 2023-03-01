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

public class MenuPanel : MonoBehaviour
{

    public bool IsOpen { get; protected set; } = false;


    public virtual void OnToggle(bool isShow)
    {
        gameObject.SetActive(isShow);
        OnStateChange(isShow);
        IsOpen = isShow;
    }

    protected virtual void OnStateChange(bool isShow)
    {      }



}

