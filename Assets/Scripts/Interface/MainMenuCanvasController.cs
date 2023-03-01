using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class MainMenuCanvasController : MonoBehaviour
{

    [SerializeField] MenuPanel Main;
    [SerializeField] MenuPanel Continue;
    [SerializeField] MenuPanel Options;
    [SerializeField] MenuPanel Gallery;

    private void Start()
    {
        Main.OnToggle(true);
    }
    public void OnNew()
    {
        GameManager.Instance.OnNewGame();
    }
    public void OnSettings()
    {
        Main.OnToggle(false);
        Gallery.OnToggle(false);
        Continue.OnToggle(false);
        Options.OnToggle(true);
    }

    public void OnGallery()
    {
        Main.OnToggle(false);
        Options.OnToggle(false);
        Continue.OnToggle(false);
        Gallery.OnToggle(true);
    }
    public void OnContinue()
    {
        Main.OnToggle(false);
        Gallery.OnToggle(false);
        Continue.OnToggle(true);
        Options.OnToggle(false);
    }    
    public void OnMain()
    {
        Main.OnToggle(true);
        Gallery.OnToggle(false);
        Continue.OnToggle(false);
        Options.OnToggle(false);
    }

    public void OnQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#endif
    }

}
