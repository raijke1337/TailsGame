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

    private void Start()
    {
        UpdateUnits();
    }
    public void UpdateUnits()
    {
        list = new List<NPCUnit>();
        _detectionArea = GetComponent<Collider>();
        _detectionArea.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var unit = other.GetComponent<NPCUnit>();
            list.Add(unit);
            unit.UnitRoom = this;
            unit.BaseUnitDiedEvent += Unit_BaseUnitDiedEvent;
            Debug.Log($"{this} registered: {unit.GetFullName}");
        }
       
    }

    private void Unit_BaseUnitDiedEvent(BaseUnit arg)
    {
        list.Remove(arg as NPCUnit);
    }

    private void LateUpdate()
    {
        if (_detectionArea.enabled) _detectionArea.enabled = false;
    }

    public void StartCombatCheck(NPCUnit unit, bool isCombat = true)
    {
        unit.EnterCombat(_player, isCombat);
    }

    // used by inputs
    public BaseUnit GetUnitForAI(UnitType type)
    {
        if (type == UnitType.Player) return _player;
        else
        {
            var res = list.ToList().FirstOrDefault(t => t.GetUnitType() == type);
            return res;
        }
    }


}


