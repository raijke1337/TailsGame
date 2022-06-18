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
    [SerializeField, Tooltip("Delay for units in room aggro")] private float _aggroDelay = 1f;
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
            Debug.Log($"{this} registered: {unit.GetFullName}");
        }
       
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
    public BaseUnit GetUnitForAI(EnemyType type, bool getPlayer = false)
    {
        if (getPlayer) return _player;
        else
        {
            return  list.ToList().FirstOrDefault(t => t.GetEnemyType == type);
        }
    }


}


