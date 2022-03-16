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

public static class Constants
{
    public const string c_TriggersConfigsPath = "/Scripts/Configurations/Triggers";
    public const string c_WeapConfigsPath = "/Scripts/Configurations/Weapons";
}



//todo some other params?
public delegate void WeaponEventHandler();

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
}


public enum WeaponType
{
    None,
    Melee,
    Ranged
}


[Serializable]
public class StatValueContainer
{
    [SerializeField] private float _start;
    [SerializeField] private float _max;
    [SerializeField] private float _min;
    [SerializeField] private float _current;
    private bool IsSetup = false;

    public SimpleEventsHandler<float> ValueChangedEvent;

    public float GetCurrent() => _current;
    public float GetMax() => _max;
    public float GetMin() => _min;
    public float GetStart() => _start;
    public void ChangeCurrent(float value)
    {
        _current = Mathf.Clamp(_current+value,_min,_max);
    }

    public void Setup()
    {
        if (!IsSetup)
        {
            _current = _start;
            IsSetup = true;
        }
    }
}



