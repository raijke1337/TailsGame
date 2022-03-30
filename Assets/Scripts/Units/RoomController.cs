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
    private List<InputsNPC> list;
    private Collider _detectionArea;

    private void Start()
    {
        list = new List<InputsNPC>();
        _detectionArea = GetComponent<Collider>();
        _detectionArea.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            list.Add(other.GetComponent<InputsNPC>());

            Debug.Log($"Found NPC {other}");
        }
    }

    private void LateUpdate()
        // find units and turn off
    {
        if (_detectionArea.enabled)
            _detectionArea.enabled = false;
    }

    public InputsNPC LookForAllies(InputsNPC controller)
    {
        if (!list.Contains(controller)) return null;

        else
        {
            return (list.First(t => t != controller));
        }
    }


}

