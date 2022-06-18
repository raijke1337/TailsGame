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
public enum Side
{
    PlayerSide,
    EnemySide
}

// all used stats 
public enum BaseStatType : byte
{
    Health,
    MoveSpeed,
    TurnSpeed
}
public enum ShieldStatType
{
    Shield,
    ShieldRegen,
    ShieldRegenMultiplier,
    ShieldAbsorbMult
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
    Cooldown,
    Speed
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

public enum TriggerTargetType
{
    TargetsEnemies,
    TargetsUser,
    TargetsAllies
}


public enum CursorType
{
    Menu,
    Explore,
    EnemyTarget,
    Item,
}
public enum UnitType : byte
{
    Small = 0,
    Big = 1,
    Boss = 2,
    Player = 255
}

public enum TextType
{
    Tutorial,
    Story
}

public enum GameMenuType
{
    Items = 0,
    Pause = 255
}

public enum InteractiveItemType : byte
{
    Enemy = 0,
    Pickup = 10,

}
