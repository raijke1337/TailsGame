using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void SimpleEventsHandler();
public delegate void SimpleEventsHandler<T>(T arg);
public delegate void SimpleEventsHandler<T1,T2>(T1 arg1,T2 arg2);

public delegate void WeaponSwitchEventHandler(EquipItemType type);
public delegate void TriggerEventApplication(string ID, BaseUnit target, BaseUnit source);
public delegate void SkillRequestedEvent(string ID, BaseUnit source, Transform where);


public delegate void StateMachineEvent();
public delegate void StateMachineEvent<T>(T arg);

public delegate void EquipmentChangeEvent<ItemContent>(bool isEquip);
public static class Constants
{
    public static class Configs
    {
        //public const string c_TriggersConfigsPath = "/Scripts/Configurations/TriggersManager/";
        //public const string c_WeapConfigsPath = "/Scripts/Configurations/WeaponsController/";
        //public const string c_BaseStatConfigsPath = "/Scripts/Configurations/BaseUnit/";
        //public const string c_EnemyStatConfigsPath = "/Scripts/Configurations/InputsNPC/";
        //public const string c_ProjectileConfigsPath = "/Scripts/Configurations/ProjectilesManager/";
        //public const string c_SkillConfigsPath = "/Scripts/Configurations/SkillsManager/";
        //public const string c_ShieldConfigsPath = "/Scripts/Configurations/ShieldController/";
        //public const string c_DodgeConfigsPath = "/Scripts/Configurations/DodgeController/";
        //public const string c_ComboConfigsPath = "/Scripts/Configurations/ComboController/";

        //public const string c_ManagerConfigsPath = "/Scripts/Configurations/";
        public const string c_AllConfigsPath = "/Scripts/Configurations/";

    }
    public static class Objects
    {
        public const string c_isoCameraTargetObjectName = "IsoCamTarget";
    }
    public static class Combat
    {
        public const string c_WeaponPrefabsPath = "/Prefabs/Weapons/";
        public const string c_SkillPrefabs = "/Prefabs/Skills/";
        public const float c_RemainsDisappearTimer = 3f;
        public const float c_StaggeringHitHealthPercent = 0.1f; // 10% max hp
    }
    public static class Texts
    {
        public const string c_TextsPath = "/Texts/";
    }
    public static class StateMachineData
    {
        public const string c_MethodPrefix = "Fsm_";
    }
}

#region structs 
[Serializable] public class Timer 
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
    public void ResetTimer(float t)
    {
        _expired = false;
        GetRemaining = t;
    }
}
[Serializable] public class StatValueContainer
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
    public IEnumerable<StatValueModifier> GetMods{get=> _mods;}

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

public class StatValueModifier:IHasID
{
    public string ID = "Not set";
    public float MaxChange;
    public float MinChange;
    public float StartChange;

    public StatValueModifier(float max,float min,float st)
    {
        MaxChange = max; MinChange = min; StartChange = st;
    }
    public string GetID => ID;
}

[Serializable] public class ProjectileData
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

[Serializable] public struct SkillData
{
    public Sprite Icon;
    public float Recharge;
    public float FinalArea;
    public float StartArea;
    public float PersistTime;
    public float SkillCost;
    public string[] TriggerIDs;

    //public TriggerTargetType TargetType;

    public SkillData(SkillData refs)
    {
        Icon = refs.Icon; Recharge = refs.Recharge;FinalArea = refs.FinalArea;StartArea = refs.StartArea;PersistTime = refs.PersistTime; SkillCost = refs.SkillCost; //TargetType = refs.TargetType;
        TriggerIDs = refs.TriggerIDs;
    }
}
[Serializable] public struct EnemyStats
{
    public float AttackRange;

    public float LookSpereCastRadius;
    public float LookSpereCastRange;

    public UnitType EnemyType;
    public EnemyStats(EnemyStatsConfig cfg)
    {
        LookSpereCastRadius = cfg.lookSphereRad;
        LookSpereCastRange = cfg.lookRange;
        AttackRange = cfg.atkRange;
        EnemyType = cfg.Type;
    }
}

[Serializable] public struct TextContainer
{
    public string ID;
    public TextType Type;
    public string[] Texts;
}
[Serializable] public struct ItemEmpties
{
    public Transform MeleeWeaponEmpty;
    public Transform RangedWeaponEmpty;
    public Transform SheathedWeaponEmpty;
    public Transform SkillsEmpty;
    public Transform OthersEmpty;
}

#endregion




#region interfaces

public interface ITakesTriggers
{
    void AddTriggeredEffect(TriggeredEffect effect);
}

public interface IStatsComponentForHandler
{
    bool IsReady { get; }
    void UpdateInDelta(float deltaTime);
    void SetupStatsComponent();
}

public interface IHasID
{ string GetID { get; } }
public interface IHasGameObject
{ public GameObject GetObject(); }
public interface IHasOwner
{ BaseUnit Owner { get; set; } }

public interface IAppliesTriggers : IHasGameObject
{
    event TriggerEventApplication TriggerApplicationRequestEvent;
}
public interface IInventoryItem : IHasID
{
    public ItemContent ItemContents { get; }
}
public interface IEquippable : IInventoryItem, IHasGameObject, IHasOwner
{
    void OnEquip(ItemContent content);
}
public interface IWeapon : IEquippable
{
    bool UseWeapon(out string reason);
    int GetAmmo { get; }
    event SimpleEventsHandler<float> TargetHit;
    void UpdateInDelta(float deltaTime);
    void SetUpWeapon(BaseWeaponConfig cfg);
}

public interface IExpires : IHasGameObject
{
    event SimpleEventsHandler<IExpires> HasExpiredEvent;
}
public interface ISkill : IHasID, IAppliesTriggers, IHasOwner , IExpires, IHasGameObject
{
    void OnUse();
    void OnUpdate();
}
public interface IProjectile : ISkill
{
    void SetProjectileData(ProjectileDataConfig cfg);
}
public interface IUsesItems : INeedsEmpties
{
    void LoadItem(IEquippable item);
}
public interface IGivesSkills : IUsesItems
{ IEnumerable<string> GetSkillStrings();  }

public interface INeedsEmpties
{ ItemEmpties Empties { get; } }



#region todo


public interface IInteractiveItem
{
    public InteractiveItemType IIType {get; }
}

#endregion



#endregion


