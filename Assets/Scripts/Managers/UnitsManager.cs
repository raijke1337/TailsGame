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
    //private TimeController _timer;
    //private UnitActivityHandler _activity;
    private List<RoomController> _rooms = new List<RoomController>();

    private List<NPCUnit> _units = new List<NPCUnit>();
    private PlayerUnit _player;
    public PlayerUnit GetPlayerUnit() => _player;
    public List<NPCUnit> GetNPCs() => _units;
    public BaseUnitWithIDEvent RequestToPlaceSkills;

    private void Awake()
    {
        _units.AddRange(FindObjectsOfType<NPCUnit>());
        _player = FindObjectOfType<PlayerUnit>();

        _rooms.AddRange(FindObjectsOfType<RoomController>());

        //_timer = new TimeController();
        //_activity = new UnitActivityHandler(_units, _player);

        SetAIStateGlobal(true);

        foreach (var npc in _units)
        {
            npc.BaseUnitDiedEvent += (t) => HandleUnitDeath(t);
            npc.SkillRequestSuccessEvent += (t1, t2) => RequestToPlaceSkills?.Invoke(t1, t2);
            npc.UnitWasAttackedEventForAggro += (t) => OnUnitAggro(t);
            
        }
        _player.BaseUnitDiedEvent += HandleUnitDeath;
        _player.SkillRequestSuccessEvent += (t1, t2) => RequestToPlaceSkills?.Invoke(t1, t2);
    }
    public void SetAIStateGlobal(bool isProcessing)
    {
        foreach (var npc in _units)
        {
            SetAIStateUnit(isProcessing, npc);
        }
    }
    public void SetAIStateUnit(bool isProcessing, NPCUnit unit)
    {
        unit.AiToggle(isProcessing);
    }
    void OnUnitAggro(NPCUnit unit)
    {
        unit.SetChaseTarget(_player);
        unit.UnitRoom.Alert(_player);
    }



    private void HandleUnitDeath(BaseUnit unit)
    {
        if (unit is NPCUnit)
        {
            SetAIStateUnit(false, unit as NPCUnit);
        }
        else
        {
            EditorApplication.isPaused = true;
            Debug.LogWarning("You died");
        }
    }

}

