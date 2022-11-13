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

public class MenuPanel : MonoBehaviour, IHasID
{

    [SerializeField] protected string _id;
    public string GetID => _id;

    public bool IsOpen { get; protected set; } = false;
    public bool StartClosed = false;
    public event SimpleEventsHandler<string,MenuPanel> SwitchToWindow;


    protected virtual void Awake()
    {
        gameObject.name = _id;
    }

    public virtual void OnToggle(bool isShow)
    {
        gameObject.SetActive(isShow);
        OnStateChange(isShow);
        IsOpen = isShow;
    }

    protected virtual void OnStateChange(bool isShow)
    {      }

    protected virtual void Start()
    {
        gameObject.SetActive(!StartClosed);
    }
       

    #region unity
    public void OtherMenuButtonClicked(string menu) => SwitchToWindow?.Invoke(menu,this);

    #endregion

}

