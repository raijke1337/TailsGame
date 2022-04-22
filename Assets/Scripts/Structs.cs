using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void SimpleEventsHandler();
public delegate void SimpleEventsHandler<T>(T arg);
public delegate void SimpleEventsHandler<T1,T2>(T1 arg1,T2 arg2);

public delegate void WeaponSwitchEventHandler(WeaponType type);
public delegate void BaseUnitWithIDEvent(string ID, BaseUnit unit);
public delegate void MouseOverEvents(InteractiveItem item, bool isSelected);


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
#if UNITY_EDITOR
        public const string c_AllConfigsPath = "/Scripts/Configurations/";
#endif
    }

    public static class Combat
    {
        public const string c_WeaponPrefabsPath = "/Prefabs/Weapons/";
        public const string c_SkillPrefabs = "/Prefabs/Skills/";
        public const float c_RemainsDisappearTimer = 3f;
        public const float c_StaggeringHitHealthPercent = 0.1f; // 10% max hp
    }

}
[Serializable] public class Timer { public float time; public Timer(float t) { time = t; } }
[Serializable] public class StatValueContainer
{
    [SerializeField,] private float _start;
    [SerializeField] private float _max;
    [SerializeField] private float _min;
    [SerializeField] private float _current;
    [SerializeField] private float _last;
    /// <summary>
    /// current, previous
    /// </summary>
    public SimpleEventsHandler<float,float> ValueChangedEvent;

    public float GetCurrent() => _current;
    public float GetMax() => _max;
    public float GetMin() => _min;
    public float GetStart() => _start;
    public float GetLast() => _last;
    /// <summary>
    /// adds the value, clamped by min and max
    /// </summary>
    /// <param name="value">use negative for dmg</param>
    public void ChangeCurrent(float value)
    {
        _last = _current;
        _current = Mathf.Clamp(_current + value, _min, _max);
        ValueChangedEvent?.Invoke(_current, _last);
    }

    public void Setup()
    {
        _current = _start;
    }
    //todo
    public StatValueContainer(StatValueContainer preset)
    {
        _start = preset._start;
        _max = preset._max == 0f ?  preset._max : preset._start;    
        _min = preset._min == 0f? preset._min : 0f;
        Setup();
    }
}


[Serializable]
public struct ProjectileData
{
    public float TimeToLive;
    public float Speed;
    public int Penetration;
    public TriggerSourceType SourceType;

    public ProjectileData(ProjectileDataConfig config)
    {
        TimeToLive = config.TimeToLive;
        Speed = config.ProjectileSpeed;
        Penetration = config.ProjectilePenetration;
        SourceType = config.SourceType;
    }
}

[Serializable]
public struct SkillData
{
    public Sprite Icon;
    public float Recharge;
    public float FinalArea;
    public float StartArea;
    public float PersistTime;
    public float SkillCost;

    public SkillTargetType TargetType;
    public TriggerSourceType SourceType;

    public SkillData(SkillData refs)
    {
        Icon = refs.Icon; Recharge = refs.Recharge;FinalArea = refs.FinalArea;StartArea = refs.StartArea;PersistTime = refs.PersistTime; SkillCost = refs.SkillCost; TargetType = refs.TargetType; SourceType = refs.SourceType; 
    }
}


[Serializable]
public struct EnemyStats
{
    public float AttackRange;
    public float TimeBetweenAttacks;

    public float LookSpereCastRadius;
    public float LookSpereCastRange;
    public float IdleTime;

    public EnemyType EnemyType;
    public EnemyStats(EnemyStatsConfig cfg)
    {
        TimeBetweenAttacks = cfg.atkCD;
        LookSpereCastRadius = cfg.lookSphereRad;
        LookSpereCastRange = cfg.lookRange;
        AttackRange = cfg.atkRange;
        IdleTime = cfg.idleTime;
        EnemyType = cfg.Type;
    }
}



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
    bool UseWeapon();
    int GetAmmo();
    string GetRelatedSkillID();
}

public interface IHasGameObject
{ public GameObject GetObject(); }

public interface InteractiveItem : IPointerEnterHandler, IPointerExitHandler
{
    public event MouseOverEvents SelectionEvent;
}

public interface IProjectile : IAppliesTriggers, ISourceMatters
{
    void OnSpawnProj();
    void OnUpdateProj();
    void OnExpiryProj();
    event SimpleEventsHandler<IProjectile> ExpiryEventProjectile;
    void SetProjectileData(ProjectileDataConfig cfg);

}
public interface IAppliesTriggers
{ event BaseUnitWithIDEvent TriggerApplicationRequestEvent; }

public interface ISourceMatters
{
    TriggerSourceType SourceType { get; }
}


#endregion


