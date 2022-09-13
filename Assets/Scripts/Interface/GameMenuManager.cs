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

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private List<MenuPanel> menuPanels = new List<MenuPanel>();
    private Dictionary<string, MenuPanel> _dict = new Dictionary<string, MenuPanel>();

    private void OnEnable()
    {
        menuPanels.AddRange(GetComponentsInChildren<MenuPanel>());
        foreach (var m in menuPanels)
        {
            _dict[m.GetID] = m;
            m.SwitchToWindow += SwitchOpenedMenu;
        }
    }
    private void SwitchOpenedMenu(string menuToOpenID,MenuPanel closed)
    {
        _dict[menuToOpenID].OnToggle(!_dict[menuToOpenID].IsOpen);
        closed.OnToggle(false);        
    }
    private void Start()
    {
        try
        {
            _dict.Values.First(t => t is MainMenuComp).OnToggle(true);
        }
        catch (InvalidOperationException e)
        {
            Debug.Log($"{e.TargetSite} had an invalid opration");
        }
    }


    // unsub
    private void OnDisable()
    {
        foreach (var m in menuPanels)
        {
            m.SwitchToWindow -= SwitchOpenedMenu;
        }
    }

}

