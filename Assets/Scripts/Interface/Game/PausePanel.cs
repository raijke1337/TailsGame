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

public class PausePanel : MenuPanel
{

    public void OnResume()
    {
        OnToggle(false);
    }
    public void OnQuit()
    {
        Debug.LogWarning("Exiting to menu");
        EditorApplication.isPlaying = false;
    }

}

