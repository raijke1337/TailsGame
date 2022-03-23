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
using UnityEngine.EventSystems;
using RotaryHeart.Lib.SerializableDictionary;

public delegate void SimpleEventsHandler();
public delegate void SimpleEventsHandler<T>(T arg);
public delegate void WeaponSwitchEventHandler(WeaponType type);

public static class Constants
{
    public const string c_TriggersConfigsPath = "/Scripts/Configurations/Triggers";
    public const string c_WeapConfigsPath = "/Scripts/Configurations/Weapons";
    public const string c_BaseStatConfigs = "/Scripts/Configurations/BaseStats"; //todo
}

#region interfaces

public interface IStatsComponentForHandler
{ 
    void UpdateInDelta(float deltaTime);
    void Setup();
}
public interface IStatsAddEffects
{
    void AddTriggeredEffect(TriggeredEffect effect);
    void AddTriggeredEffects(IEnumerable<TriggeredEffect> effects);
}

public interface IWeapon
{    
    bool UseWeapon();
    public GameObject GetObject();
    int GetAmmo();
}

#endregion
[Serializable] public class Timer { public float time; public Timer(float t) { time = t; } }


[Serializable]
public class StatValueContainer
{
    [SerializeField,] private float _start;
    [SerializeField] private float _max;
    [SerializeField] private float _min;
    [SerializeField] private float _current;

    public SimpleEventsHandler<float> ValueDecreasedEvent;

    public float GetCurrent() => _current;
    public float GetMax() => _max;
    public float GetMin() => _min;
    //public float GetStart() => _start;
    // dont need because when properly set up current == start
    /// <summary>
    /// adds the value
    /// </summary>
    /// <param name="value">how much to add or remove</param>
    public void ChangeCurrent(float value)
    {
        _current = Mathf.Clamp(_current+value,_min,_max);
        if (value < 0f)
        ValueDecreasedEvent?.Invoke(_current);
    }

    public void Setup()
    {
        _current = _start;
    }
    //todo
    public StatValueContainer(StatValueContainer preset)
    {
        _start = preset._start;
        _max = preset._max;
        _min = preset._min;
        Setup();
    }
}



