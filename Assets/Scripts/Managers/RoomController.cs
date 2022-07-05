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

public class RoomController : MonoBehaviour
{
    [SerializeField] private List<NPCUnit> list;
    private Collider _detectionArea;
    private PlayerUnit _player;
    public void SetPlayer(PlayerUnit p) => _player = p;
    private Puzzle.PuzzleManager _puzMan;



    private void Start() 
    {
        UpdateUnits();
        _puzMan = FindObjectOfType<Puzzle.PuzzleManager>();
        if (_puzMan != null)
        {
            _puzMan.GameCompletedEvent += _puzMan_GameCompletedEvent;
        }
    }

    private void _puzMan_GameCompletedEvent(Puzzle.GameResult arg)
    {
        Debug.Log($"Puzzle solved {arg.isWin}");
    }

    private void LateUpdate() 
    {
        if (_detectionArea.enabled) _detectionArea.enabled = false;
    }

    #region units


    public void UpdateUnits()
    {
        list = new List<NPCUnit>();
        _detectionArea = GetComponent<Collider>();
        if (_detectionArea != null) _detectionArea.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var unit = other.GetComponent<NPCUnit>();
            list.Add(unit);
            unit.SetUnitRoom(this);
            unit.BaseUnitDiedEvent += Unit_BaseUnitDiedEvent;
            unit.OnUnitSpottedPlayerEvent += Unit_OnUnitSpottedPlayerEvent;
            unit.OnUnitAttackedEvent += Unit_OnUnitAttackedEvent;
        }
       
    }
    #region unit events

    private void Unit_OnUnitAttackedEvent(NPCUnit arg)
    {
        arg.ReactToDamage(_player);
        Debug.Log($"{this}: {arg} was attacked");
    }

    private void Unit_OnUnitSpottedPlayerEvent(NPCUnit arg)
    {
        Debug.Log($"{this}: {arg} saw player");
    }

    private void Unit_BaseUnitDiedEvent(BaseUnit unit)
    {
        list.Remove(unit as NPCUnit);
        if(unit is NPCUnit n)
        {
            n.OnUnitSpottedPlayerEvent -= Unit_OnUnitSpottedPlayerEvent;
            n.OnUnitAttackedEvent -= Unit_OnUnitAttackedEvent;
        }
    }

    #endregion



    // used by inputs
    public BaseUnit GetUnitForAI(UnitType type)
    {
        BaseUnit res = null;
        switch (type)
        {
            case UnitType.Small:
                res = list.ToList().FirstOrDefault(t => t.GetUnitType() == type);
                break;
            case UnitType.Big:
                res= list.ToList().FirstOrDefault(t => t.GetUnitType() == type);
                break;
            case UnitType.Boss:
                res= list.ToList().FirstOrDefault(t => t.GetUnitType() == type);
                break;
            case UnitType.Self:
                Debug.LogWarning(type+" was somehow requested, this should not happen");
                break;
            case UnitType.Any:
                Debug.LogWarning(type + " NYI");
                break;
            case UnitType.Player:
                res = _player;
                break;
        }
        return res;
    }

    #endregion

    #region puzzles




    #endregion

}


