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

public class TextTrigger : BaseTrigger
{
    public string TextID;
    public SimpleEventsHandler<string,bool> ShowTextEvent;


    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ShowTextEvent?.Invoke(TextID, true);
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ShowTextEvent?.Invoke(TextID, false);
    }
}

