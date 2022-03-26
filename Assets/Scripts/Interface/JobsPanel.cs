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

public class TasksPanel : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Image _statusIcon;
    [SerializeField] private Color progressColor = Color.black;
    [SerializeField] private Color doneColor = Color.green;


    private void Start()
    {
        _text = GetComponentInChildren<Text>();
    }
    public void JobStart(string text)
    {
        Debug.Log("New job: " + text);
    }
    public void JobComplete()
    {
        Debug.Log("Job completed");
    }
}

