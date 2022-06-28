using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void SimpleEventsHandler();
public delegate void SimpleEventsHandler<T>(T arg);
public delegate void SimpleEventsHandler<T1,T2>(T1 arg1,T2 arg2);

public delegate void WeaponSwitchEventHandler(WeaponType type);
public delegate void TriggerEventApplication(string ID, BaseUnit target, BaseUnit source);
public delegate void SkillRequestedEvent(string ID, BaseUnit source);


public delegate void StateMachineEvent();
public delegate void StateMachineEvent<T>(T arg);

public static class Constants
{
    public static class Configs
    {
        public const string c_TriggersConfigsPath = "/Scripts/Configurations/TriggersManager/";
        public const string c_WeapConfigsPath = "/Scripts/Configurations/WeaponsController/";
        public const string c_BaseStatConfigsPath = "/Scripts/Configurations/BaseUnit/";
        public const string c_EnemyStatConfigsPath = "/Scripts/Configurations/InputsNPC/";
        public const string c_ProjectileConfigsPath = "/Scripts/Configurations/ProjectilesManager/";
        public const string c_SkillConfigsPath = "/Scripts/Configurations/SkillsManager/";
        public const string c_ShieldConfigsPath = "/Scripts/Configurations/ShieldController/";
        public const string c_DodgeConfigsPath = "/Scripts/Configurations/DodgeController/";
        public const string c_ComboConfigsPath = "/Scripts/Configurations/ComboController/";

        public const string c_ManagerConfigsPath = "/Scripts/Configurations/";

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
    private float _current;
    private float _last;
    /// <summary>
    /// current, previous
    /// </summary>
    public SimpleEventsHandler<float,float> ValueChangedEvent;

    public float GetCurrent { get => _current; }
    public float GetMax { get => _max; }
    public float GetMin {get => _min;}
    public float GetStart {get => _start;}
    public float GetLast {get => _last;}

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
    }

    public StatValueContainer(StatValueContainer preset)
    {
        _start = preset._start;
        _max = preset._max;
        _min = preset._min;
        Setup();
    }
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

#endregion

#region interfaces

public interface IStatsComponentForHandler
{
    void UpdateInDelta(float deltaTime);
    void SetupStatsComponent();
}
public interface IStatsAddEffects
{
    void AddTriggeredEffect(TriggeredEffect effect);
}
public interface IWeapon : IHasGameObject
{
    BaseUnit Owner { get; set; }
    bool UseWeapon(out string reason);
    void UpdateInDelta(float deltaTime);
    int GetAmmo { get; }
    string GetRelatedSkillID();
    event SimpleEventsHandler<float> TargetHit;
}

public interface IHasGameObject
{ public GameObject GetObject(); }


public interface IProjectile : IAppliesTriggers, IHasID
{
    void OnSpawnProj();
    void OnUpdateProj();
    void OnExpiryProj();
    event SimpleEventsHandler<IProjectile> ExpiryEventProjectile;
    void SetProjectileData(ProjectileDataConfig cfg);
    BaseUnit Source { get; }
}
public interface IHasID
{ string GetID { get; } }


public interface IAppliesTriggers
{ event TriggerEventApplication TriggerApplicationRequestEvent; }


public interface IInteractiveItem
{
    public InteractiveItemType IIType {get;}

}



#endregion


