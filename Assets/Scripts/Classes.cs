using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;


[Serializable]
public class AudioComponentBase
{
    [SerializeField] public SerializedDictionary<SoundType, AudioClip> SoundsDict;
}


[XmlRoot("GameSave"), Serializable]
public class SaveData
{
    public List<string> OpenedLevels;
    public UnitInventoryItems PlayerItems;

    public SaveData(List<string> levels, UnitInventoryItems items)
    {
        OpenedLevels = new List<string>();
        foreach (var l in levels)
        {
            if (!OpenedLevels.Contains(l)) OpenedLevels.Add(l);
        }
        PlayerItems = items;
    }
    public SaveData()
    {
        OpenedLevels = new List<string>();
    }

}
public static class Constants
{
    public static class Configs
    {
        public const string c_AllConfigsPath = "/Resources/Configurations/";
        public const string c_SavesPath = "/Saves/Save.xml";
        public const string c_LevelsPath = "/Resources/Configurations/LevelCards";
        public const string c_FirstLevelID = "debug";
    }
    public static class Objects
    {
        public const string c_isoCameraTargetObjectName = "IsoCamTarget";
    }
    public static class Combat
    {
        public const float c_RemainsDisappearTimer = 3f;
        public const float c_StaggeringHitHealthPercent = 0.1f; // 10% max hp
    }
    public static class PrefabsPaths
    {
        public const string c_ItemPrefabsPath = "/Resources/Prefabs/Items/";
        public const string c_SkillPrefabs = "/Resources/Prefabs/Skills/";
        public const string c_InterfacePrefabs = "/Resources/Prefabs/Interface/";
    }
    public static class Texts
    {
        public const string c_TextsPath = "/Resources/Texts/";
    }
    public static class StateMachineData
    {
        public const string c_MethodPrefix = "Fsm_";
    }

}
[Serializable]
public class Timer
{
    public delegate void TimerEvents<T>(T arg) where T : Timer;
    public event TimerEvents<Timer> TimeUp;
    public float GetInitial { get; }
    public float GetRemaining { get; private set; }
    public bool GetExpired { get => _expired; }
    private bool _expired;
    public Timer(float t)
    {
        GetInitial = t;
        GetRemaining = t;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="deltatime"></param>
    /// <returns> current remaining time</returns>
    public float TimerTick(float deltatime)
    {
        if (GetRemaining <= 0 && !_expired)
        {
            _expired = true;
            TimeUp?.Invoke(this);
        }
        else if (GetRemaining > 0)
        {
            GetRemaining -= deltatime;
        }
        return GetRemaining;
    }
    public void ResetTimer()
    {
        _expired = false;
        GetRemaining = GetInitial;
    }
}
[Serializable]
public class StatValueContainer
{
    [SerializeField] private float _start;
    [SerializeField] private float _max;
    [SerializeField] private float _min;



    private float _calcStart;
    private float _calcMax;
    private float _calcMin;
    private float _current;
    private float _last;
    private List<StatValueModifier> _mods;

    /// <summary>
    /// current, previous
    /// </summary>
    public SimpleEventsHandler<float, float> ValueChangedEvent;

    public float GetCurrent { get => _current; }
    public float GetMax { get => _calcMax; }
    public float GetMin { get => _calcMin; }
    public float GetStart { get => _calcStart; }
    public float GetLast { get => _last; }
    public IEnumerable<StatValueModifier> GetMods { get => _mods; }

    /// <summary>
    /// adds the value, clamped by min and max
    /// </summary>
    /// <param name="value">use negative for decrease</param>
    public void ChangeCurrent(float value)
    {
        _last = _current;
        _current = Mathf.Clamp(_current + value, _min, _max);
        ValueChangedEvent?.Invoke(_current, _last);
    }

    public void Setup()
    {
        _current = _start;
        // this is for lazy people
        // case: only start value is set
        if (_max == 0) _max = _start;
        _mods = new List<StatValueModifier>();
        RefreshValues();
    }
    public void AddMod(StatValueModifier mod)
    {
        _mods.Add(mod);
    }
    public void RemoveMod(StatValueModifier mod)
    {
        _mods.Remove(mod);
    }
    public void RemoveMod(string modID)
    {
        var f = _mods.First(t => t.ID == modID);
        if (f == null) return;
        _mods.Remove(f);
    }
    private void RefreshValues()
    {
        _calcMin = _min + _mods.Sum(t => t.MinChange);
        if (_calcMin <= 0f)
        {
            ValueChangedEvent?.Invoke(0f, _min);
        }
        _calcMax = _max + _mods.Sum(t => t.MaxChange);
        _calcStart = _start + _mods.Sum(t => t.StartChange);
    }


    public StatValueContainer(StatValueContainer preset)
    {
        _start = preset._start;
        _max = preset._max;
        _min = preset._min;
        Setup();
    }
}

public class StatValueModifier : IHasID
{
    public string ID = "Not set";
    public float MaxChange;
    public float MinChange;
    public float StartChange;

    public StatValueModifier(float max, float min, float st)
    {
        MaxChange = max; MinChange = min; StartChange = st;
    }
    public string GetID => ID;
}

[Serializable]
public class ProjectileData
{
    public float TimeToLive;
    public float Speed;
    public int Penetration;

    public ProjectileData(ProjectileDataConfig config)
    {
        TimeToLive = config.TimeToLive;
        Speed = config.ProjectileSpeed;
        Penetration = config.ProjectilePenetration;
    }
}
[Serializable]
public class UnitInventoryItems
{
    public List<string> EquipmentIDs;
    public List<string> InventoryIDs;
    public List<string> DropsIDs;
    public UnitInventoryItems(UnitItemsSO cfg)
    {
        EquipmentIDs = new List<string>(cfg.Items.EquipmentIDs);
        InventoryIDs = new List<string>(cfg.Items.InventoryIDs);
        DropsIDs = new List<string>(cfg.Items.DropsIDs);
    }
    public UnitInventoryItems()
    {
        EquipmentIDs = new List<string>(); InventoryIDs = new List<string>();
        DropsIDs = new List<string>();
    }
}
[Serializable]
public class LevelData
{
    public string LevelID;
    public int SceneLoaderIndex;
    public string NextLevelID;
    [Space]
    public string LevelNameShort;
    public string DescriptionContainer;
    public LevelType Type;
    public AudioClip Music;

    public LevelData(LevelCardSO data)
    {
        LevelID = data.ID;
        SceneLoaderIndex = data.SceneLoaderIndex;
        LevelNameShort = data.LevelNameShort;
        DescriptionContainer = data.TextContainerID;
        Type = data.LevelType;
        NextLevelID = data.nextID;
        Music = data.Music;
    }
}