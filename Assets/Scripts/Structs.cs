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
using RotaryHeart;
using RotaryHeart.Lib.SerializableDictionary;

public delegate void SimpleEventsHandler();
public delegate void SimpleEventsHandler<T>(T arg);

//side
public enum Allegiance
{
    Ally,
    Enemy
}
//for anim debug
public class MovementDebugData
{
    public Vector3 _facing;
    public Vector3 _movement;
    public Vector3 _animVector;
}


// dictionary for statshandler
// stores default stat values
[Serializable]
public class StatsDictionary : SerializableDictionaryBase<StatType, StatContainer> {}

// all used stats 
public enum StatType
{
    Health,
    Shield,
    MoveSpeed,
    CritChance,
    CritMult,
    DashRange,
    DashCount,
    Heat,
    HealthRegen,
    ShieldRegen,
    HeatRegen
}
// todo separate stats for all, stats only for player and stats only for enemies

// contains:
// stat range data
// current and default value
// all stat modifiers
[Serializable]
public class StatContainer
{
    [SerializeField]
    private float _defaultValue;
    // set in editor

    public StatRange Range;


    public float GetCurrentValue { get; private set; }
    // returns result of all mods applied
    // recalculated often

    private LinkedList<StatModData> _statMods = new LinkedList<StatModData>();

    public void AddStatMod(StatModData data)
    {
        _statMods.AddLast(data);
        UpdateStatValue();
    }
    public void RemoveStatMod(string ID)
    {
        var stat = _statMods.First(t => t.ID == ID);
        _statMods.Remove(stat);
        UpdateStatValue();
    }

    public void UpdateStatValue()
    {
        // gets the sum of all mods in the list
        if (_statMods == null)
        {
            GetCurrentValue = _defaultValue;
        }
        else
        {
            GetCurrentValue = _defaultValue + _statMods.Sum(t => t.Value);
        }         
    }
}



//this is used by stat handler
[Serializable]
public struct StatModData
{
    public StatType Type;
    public float Value;
    public string ID;
    public StatModData(StatType type, float value, string id)
    {
        Type = type; Value = value; ID = id;
    }
}


// not implemented for now
// todo make an editor to use range info for editor setting
public struct StatRange
{
    public float Min;
    public float Max;
    public bool IsInit;


    public StatRange(float min, float max)
    {
        Min = min; Max = max; IsInit = true;
    }
    public override string ToString()
    {
        return string.Concat($"Min: {Min} Max: {Max}");
    }
    public override bool Equals(object obj)
    {
        if (!(obj is StatRange)) return false;
        var _range = (StatRange)obj;
        return _range.Max == Max && _range.Min == Min;
    }
    public override int GetHashCode()
    {
        var hashCode = -1998946679;
        hashCode = hashCode * -1521134295 + Min.GetHashCode();
        hashCode = hashCode * -1521134295 + Max.GetHashCode();
        return hashCode;
    }
}



// used by commands applied by weapons traps healing etc
[Serializable]
public struct EffectData
{
    public float Duration;
    public string ID;
    public Sprite Sprite;

    public EffectData(float dur, string id, Sprite sp)
    {
        Duration = dur; ID = id; Sprite = sp;
    }
}
