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

//side
public enum Allegiance
{
    Ally,
    Enemy
}

// all used stats 
public enum StatType
{
    Health,
    Shield,
    HealthRegen,
    ShieldRegenMultiplier,
    Heat,
    HeatRegen,
    MoveSpeed
}
public enum TriggeredEffectTargetType
{
    Target,
    Self
}

public enum WeaponType
{
    None,
    Melee,
    Ranged
}
public enum DodgeStatType
{
    Charges,
    Range,
    Duration,
    Cooldown
}

public enum CombatActionType
{
    Melee,
    Ranged,
    Dodge,
    MeleeSpecialQ,
    RangedSpecialE,
    ShieldSpecialR
}

public enum CursorType
{
    Menu,
    Explore,
    EnemyTarget,
    Item,
}