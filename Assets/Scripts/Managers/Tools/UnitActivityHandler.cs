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

public class UnitActivityHandler
{
    private List<NPCUnitControllerAI> _units;
    private PlayerUnitController _player;

    public UnitActivityHandler(IEnumerable<NPCUnitControllerAI> units, PlayerUnitController player)
    {
        _units = new List<NPCUnitControllerAI>(); _player = player;
        _units.AddRange(units);
        foreach (var unit in units)
        {
            unit.NPCdiedEvent += (t) => SetAIStateUnit(false,t);
        }
    }

    public void SetAIStateGlobal(bool isProcessing)
    {
        foreach (var npc in _units)
        {
            SetAIStateUnit(isProcessing, npc);
        }
    }
    public void SetAIStateUnit(bool isProcessing, NPCUnitControllerAI unit)
    {
        unit.SetAI(isProcessing);
    }



}


