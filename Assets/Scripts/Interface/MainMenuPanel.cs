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

public class MainMenuPanel : InterfaceComp
{
    public void OnStart()
    {
        SceneManager.LoadScene("Level01");
    }
    public void OnSettings()
    {
        Debug.Log("Settings not implemented yet");
    }
    public void OnQuit()
    {
        EditorApplication.isPlaying = false;
    }


}

