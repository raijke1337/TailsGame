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
    private List<InputsNPC> _units;
    private InputsPlayer _player;

    public UnitActivityHandler(IEnumerable<InputsNPC> units, InputsPlayer player)
    {
        _units = new List<InputsNPC>(); _player = player;
        _units.AddRange(units);
    }

    public void SetAIStateGlobal(bool isProcessing)
    {
        foreach (var npc in _units)
        {
            SetAIStateUnit(isProcessing, npc);
        }
    }
    public void SetAIStateUnit(bool isProcessing, InputsNPC unit)
    {
        unit.SetAI(isProcessing);
    }



}


