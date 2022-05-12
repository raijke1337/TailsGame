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

public class TriggeredEffect 
{
    public string ID; // used to load info
    public BaseStatType StatID; // used to pick changed stat
    public float InitialValue; 
    public float RepeatedValue; //tick value
    public float RepeatApplicationDelay { get; }
    public float TotalDuration; 
    public Sprite Icon;
    public TriggerSourceType Source;


    public float CurrentRepeatTimer; // used by stats controller to do ticks
    public bool InitialDone = false; // is initial value applied

    public TriggeredEffect (string id,BaseStatType type,float init,float repeat = 0,float repeatDelay = 0,float totalDuration = 0, Sprite icon = null)
    {
        ID = id; StatID = type; InitialValue = init;RepeatedValue = repeat;RepeatApplicationDelay = repeatDelay;TotalDuration = totalDuration; Icon = icon;
        CurrentRepeatTimer = RepeatApplicationDelay;
    }
    public TriggeredEffect(BaseStatTriggerConfig config)
    {
        ID = config.ID; StatID = config.StatID; InitialValue = config.InitialValue; RepeatedValue = config.RepeatedValue; RepeatApplicationDelay = config.RepeatApplicationDelay; TotalDuration = config.TotalDuration; Icon = config.Icon;
        CurrentRepeatTimer = RepeatApplicationDelay;  Source = config.SourceType;
    }

}

