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

[CreateAssetMenu(fileName = "New BaseStatTriggerConfiguration",menuName = "Configurations/BaseStatTrigger",order =1 )]
public class BaseStatTriggerConfig : ScriptableObjectID
{
    public TriggerChangedValue ChangedValueType;
    public float InitialValue;
    public float RepeatedValue;
    public float RepeatApplicationDelay;
    public float TotalDuration;
    public Sprite Icon;

    public TriggerTargetType TargetType;
}

