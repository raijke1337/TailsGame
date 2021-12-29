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


public interface IInteractiveItem
{
    /// <summary>
    /// Interact with item
    /// </summary>
    /// <returns>bool means success of interaction</returns>
    bool Interact();
}



public delegate void SimpleEventsHandler();
public delegate void FocusEventHandler(BaseStats component, bool isSelect);



public enum Allegiance
{
    Ally,
    Enemy
}

public class MovementDebugData
{
    public Vector3 _facing;
    public Vector3 _movement;
    public Vector3 _animVector;
}

public enum WeaponSpecial
{
    Projectile,
    Knockback
}
public enum WeaponType
{
    Melee,
    Ranged
}

[Serializable]
public struct WeaponStats
{
    string Name;
    public WeaponType Type;
    public int Damage;
    public WeaponSpecial Special;
    public int ID;
    public WeaponStats(string name, WeaponType type, int dmg, WeaponSpecial spec, int id)
    {
        Name = name;
        Type = type;
        Damage = dmg;
        Special = spec;
        ID = id;
    }
}




