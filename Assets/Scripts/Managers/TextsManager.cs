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
using System.Text;
using TMPro;

public class TextsManager : SingletonManagerBase
{
    private List<TextTrigger> triggers = new List<TextTrigger>();
    private Dictionary<string, TextContainer> textContainers = new Dictionary<string, TextContainer>();


    [SerializeField] private GameObject TextPanel;
    private Text Txt;

    private void ShowTexts(string ID,bool isShow)
    {
        TextPanel.SetActive(isShow);
        string result = string.Join("\n", textContainers[ID].Texts);
        Txt.text = result;
    }

    public TextContainer GetContainerByID(string ID) => textContainers[ID];

    public override void SetupManager()
    {
        if (TextPanel == null)
        { Debug.LogWarning($"Set up the panel ref {this}");
            return;
        } 
        Txt = TextPanel.GetComponentInChildren<Text>();
        triggers.AddRange(FindObjectsOfType<TextTrigger>());
        foreach (var trigger in triggers)
        {
            trigger.ShowTextEvent += ShowTexts;
        }
        TextPanel.SetActive(false);
        var texts = Extensions.GetAssetsOfType<TextContainerSO>(Constants.Texts.c_TextsPath);

        foreach (var text in texts)
        {
            textContainers[text.container.ID] = text.container;
        }
        Debug.Log($"Found total {textContainers.Count} texts");

        Txt.text = "";
        TextPanel.SetActive(false);
    }


    #region SingletonLogic

    protected static TextsManager _instance = null;
    public override void InitSingleton()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance == this) Destroy(gameObject); // remove copies just in case
        SetupManager();
    }
    public static TextsManager GetInstance
    {
        get
        {
            return _instance;
        }
    }
    #endregion
}

