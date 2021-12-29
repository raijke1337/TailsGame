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

public class GameManager : MonoBehaviour
{

    internal static GameManager _self;
    private void SingletonLogic()
    {
        if (_self != null)
        {
            Destroy(this);
        }
        else
        {
            _self = this;
        }
    }

    private InGameUIManager _uiMan;

    [SerializeField]
    private Dictionary<int, WeaponStats> WeaponsDB;
    public WeaponStats GetWeaponStats(int ID) => WeaponsDB[ID];

    public Unit GetPlayerUnit { get; private set; }
    private List<BaseStats> _itemsWithStats = new List<BaseStats>();

    private void Awake()
    {
        SingletonLogic();
        GetPlayerUnit = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
        if (GetPlayerUnit == null)
        {
            Debug.LogError("Missing player in scene");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        _uiMan = FindObjectOfType<InGameUIManager>();
        SetupWeaponsDB();
    }

    private void SetupWeaponsDB()
    {
        WeaponsDB = new Dictionary<int, WeaponStats>()
        {
            {1,new WeaponStats("Hammer",WeaponType.Melee,25,WeaponSpecial.Knockback,1) },
            {2,new WeaponStats("Bow",WeaponType.Ranged,15,WeaponSpecial.Projectile,2) },
        };
    }
    private void Start()
    {
        _itemsWithStats.AddRange(FindObjectsOfType<BaseStats>());
        if (_itemsWithStats != null) _uiMan.SubscriveToEventsInBaseStats(_itemsWithStats);
    }




}

