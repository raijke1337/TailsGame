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
    public UnitsManager Manager;

    Timer _timer;
    Coroutine agCor;


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
            Debug.Log($"{this} registered: {unit.GetFullName()}");
        }
       
    }

    private void LateUpdate()
    {
        if (_detectionArea.enabled) _detectionArea.enabled = false;
        if (_timer == null) _timer = new Timer(_aggroDelay);
        _timer.TimerTick(Time.deltaTime);
    }

    public NPCUnit CallBigRobot()
    {
        var result = list.FirstOrDefault(t => t.GetEnemyType == EnemyType.Big);
        if (result != null)
        {
            StartCombatCheck(result,true);
        }
        return result;
    }
    public void StartCombatCheck(NPCUnit unit, bool isCombat = true)
    {
        unit.EnterCombat(Manager.GetPlayerUnit, isCombat);
        if (agCor == null) agCor = StartCoroutine(AggroCor());
    }

    private IEnumerator AggroCor()
    {
        int index = 0;
        while (!_timer.GetExpired) yield return null;
        while (index < list.Count)
        {
            list[index].EnterCombat(Manager.GetPlayerUnit, true);
            index++;
            _timer.ResetTimer();
            yield return null;
        }
    }
}


