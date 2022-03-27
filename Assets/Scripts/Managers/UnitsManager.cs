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

    private List<NPCUnitControllerAI> _units = new List<NPCUnitControllerAI>();
    private PlayerUnitController _player;

    private void Start()
    {
        _units.AddRange(FindObjectsOfType<NPCUnitControllerAI>());
        _player = FindObjectOfType<PlayerUnitController>();

        _timer = new TimeController();
        _activity = new UnitActivityHandler(_units, _player);

        _activity.SetAIStateGlobal(true);

        foreach (var npc in _units)
        {
            npc.NPCdiedEvent += (t) => DeadNPCaiHandling(t);
        }
    }

    private void DeadNPCaiHandling(NPCUnitControllerAI npc)
    {
        npc.SetAI(false);
        _units.Remove(npc);
    }

    // todo very inefficient
    // maybe just run it once in a while (use timeinstate)
    public bool GetNearestAllyLocation(Vector3 position, out Vector3 location)
    {
        location = Vector3.zero;
        if (_units.Count <= 1) return false;

        float leastDistance = float.MaxValue;

        foreach (var unit in _units)
        {
            var newDistance = Vector3.Distance(position, unit.transform.position);
            if (newDistance < leastDistance)
            {
                location = unit.transform.position;
            }
        }
        return true;
    }



}

