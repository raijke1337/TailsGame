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
using UnityEngine.AI;

public class navmeshtest : MonoBehaviour
{
    public Transform[] target;
    NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
    }

    private void Update()
    {
        agent.SetDestination(target[0].position);
    }
}

