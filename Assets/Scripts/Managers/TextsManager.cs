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

public class TextsManager : MonoBehaviour
{
    private List<TextTrigger> triggers = new List<TextTrigger>();
    private List<TextContainer> containers = new List<TextContainer>();
    [SerializeField] private GameObject TextPanel;
    private Text Txt;
    private void Awake()
    {
        if (TextPanel == null) Debug.LogError($"Set up the panel ref {this}");
        Txt = TextPanel.GetComponentInChildren<Text>();
        triggers.AddRange(FindObjectsOfType<TextTrigger>());
        foreach (var trigger in triggers)
        {
            trigger.ShowTextEvent += ShowTexts;
        }
        TextPanel.SetActive(false);
        var ids = Extensions.GetAssetsOfType<TextContainerSO>(Constants.Texts.c_TextsPath);
        foreach (var id in ids)
        {
            containers.Add(id.container);
        }
        Txt.text = "";
        TextPanel.SetActive(false);
    }
    private void ShowTexts(string ID,bool isShow)
    {
        TextPanel.SetActive(isShow);
        var cont = containers.First(t => t.ID == ID);
        string result = string.Join("\n", cont.Texts);
        Txt.text = result;
    }

}

