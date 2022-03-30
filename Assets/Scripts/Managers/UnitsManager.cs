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

    private List<InputsNPC> _units = new List<InputsNPC>();
    private InputsPlayer _player;
    private List<RoomController> _rooms = new List<RoomController>();

    private void Start()
    {
        _units.AddRange(FindObjectsOfType<InputsNPC>());
        _player = FindObjectOfType<InputsPlayer>();

        _rooms.AddRange(FindObjectsOfType<RoomController>());

        _timer = new TimeController();
        _activity = new UnitActivityHandler(_units, _player);

        _activity.SetAIStateGlobal(true);

        foreach (var npc in _units)
        {
            npc.NPCdiedDisableAIEvent += (t) => _activity.SetAIStateUnit(false,t);
        }
    }

    // todo
    // maybe just run it once in a while (use timeinstate)
    public bool AreAlliesInRoom(InputsNPC controller, out InputsNPC unit)
    {
        InputsNPC result = null;
        foreach (var room in _rooms)
        {
            result = room.LookForAllies(controller);
        }
        unit = result;
        return unit != null;
    }



}

