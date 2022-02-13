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

public abstract class BaseCommand
{
    private float _duration;
    public Unit Target { get; set; }
    public Unit Source { get; set; }

    public float CurrentDuration { get; set; }

    public float Duration { get => _duration; set => _duration = CurrentDuration = value; }

    public string ID { get; }
    public Sprite Sprite { get; }

    public BaseCommand(EffectData data)
    {
        Duration = data.Duration; Sprite = data.Sprite; ID = data.ID;
    }

}


public interface IStartCommand
{
    void OnStartCommand();
}
public interface IEndCommand
{
    void OnEndCommand();
}
public interface IUpdateCommand
{
    void OnUpdateCommand(float delta);
}