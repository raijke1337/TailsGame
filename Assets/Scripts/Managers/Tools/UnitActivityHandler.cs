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
    private List<NPCUnit> _units;
    private PlayerUnit _player;

    public UnitActivityHandler(IEnumerable<NPCUnit> units, PlayerUnit player)
    {
        _units = new List<NPCUnit>(); _player = player;
        _units.AddRange(units);
    }

    //public void SetAIStateGlobal(bool isProcessing)
    //{
    //    foreach (var npc in _units)
    //    {
    //        SetAIStateUnit(isProcessing, npc);
    //    }
    //}
    //public void SetAIStateUnit(bool isProcessing, NPCUnit unit)
    //{
    //    unit.GetInputs<InputsNPC>().SetAI(isProcessing);
    //    unit.GetInputs<InputsNPC>().NavMeshAg.isStopped = isProcessing;
    //}



}


