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

public class UnitsManager : MonoBehaviour
{
    private TimeController _timer;
    private UnitActivityHandler _activity;

    private List<NPCUnit> _units = new List<NPCUnit>();
    private PlayerUnit _player;
    private List<RoomController> _rooms = new List<RoomController>();

    public PlayerUnit GetPlayerUnit() => _player;
    public List<NPCUnit> GetNPCs() => _units;


    private void Awake()
    {
        _units.AddRange(FindObjectsOfType<NPCUnit>());
        _player = FindObjectOfType<PlayerUnit>();

        _rooms.AddRange(FindObjectsOfType<RoomController>());

        _timer = new TimeController();
        _activity = new UnitActivityHandler(_units, _player);

        _activity.SetAIStateGlobal(true);

        foreach (var npc in _units)
        {
            npc.BaseUnitDiedEvent += (t) => HandleUnitDeath(t);
        }
        _player.BaseUnitDiedEvent += HandleUnitDeath;
    }

    private void HandleUnitDeath(BaseUnit unit)
    {
        Debug.Log($"{unit.GetFullName()} died, add some logic to manager");
    }



}

