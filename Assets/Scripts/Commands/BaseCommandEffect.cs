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
using static BaseCommandEffect;


public abstract class BaseCommandEffect : BaseCommand<BaseCommandEffect>
{
    private float _duration;
    public Unit Target { get; set; }
    public Unit Source { get; set; }

    public float CurrentDuration { get; set; }

    public float Duration { get => _duration; set => _duration = CurrentDuration = value; }

    public string ID { get; }
    public Sprite Sprite { get; }

    public BaseCommandEffect(EffectData data)
    {
        Duration = data.Duration; Sprite = data.Sprite; ID = data.ID;
    }
}

public abstract class BaseCommand <T> where T : class
{
    public abstract void OnStart();
    public abstract void OnUpdate(float delta);
    public abstract void OnEnd();
    public abstract T Clone();
}
