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

public class InGameUIManager : MonoBehaviour
{
    [SerializeField,Tooltip("Time for target info bar to fade away in")]
    private float FadeOutTimer =1f;

    private TargetInfoPanel _targetPanel;
    private void Start()
    {
        _targetPanel = GetComponentInChildren<TargetInfoPanel>();
        if (_targetPanel == null) Debug.LogError("Missing TargetInfoPanel comp");
    }

    //public void SubscriveToEventsInBaseStats(IEnumerable statsArray)
    //{
    //    foreach (BaseStats item in statsArray)
    //    {
    //        item.OnFocusEventHandler += BaseStatsMouseOverEvent;
    //    }
    //}

    //// for Basestats objects events subs
    //private void BaseStatsMouseOverEvent(BaseStats component, bool isSelect)
    //{
    //    if (!isSelect)
    //    {
    //        _targetPanel.ToggleActiveState();
    //    }
    //    else
    //    {
    //        if(!_targetPanel.IsVisible)
    //        {
    //            _targetPanel.ToggleActiveState();
    //            _targetPanel.LoadedStats = component;
    //        }
    //        _targetPanel.LoadedStats = component;
    //    }

    //}





}

