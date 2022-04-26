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

    private void Start()
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
    }
    public void Alert(PlayerUnit player)
    {
        foreach (var unit in list) unit.SetChaseTarget(player);
    }

    public NPCUnit GetBigRobot()
    {
        var result = list.FirstOrDefault(t => t.GetEnemyType == EnemyType.Big);
        return result;
    }


}


