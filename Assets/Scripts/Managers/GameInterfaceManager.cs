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
using Zenject;
using System.Threading.Tasks;

public class GameInterfaceManager : MonoBehaviour
{
    [SerializeField] private TargetUnitPanel _tgt;
    [SerializeField] private PlayerUnitPanel _player;
    [SerializeField] private MenuPanel _items;
    [SerializeField] private MenuPanel _pause;

    private UnitsManager _unitsM;

    private async void Start()
    {
        await Task.Yield();

        _unitsM = FindObjectOfType<UnitsManager>();
        _player.AssignItem(_unitsM.GetPlayerUnit,true);
        _unitsM.GetPlayerUnit.SetInfoPanel(_tgt);
        _unitsM.GetPlayerUnit.ToggleMenuEvent += GameInterfaceManager_ToggleMenuEvent;


        // hides the menus
        _items.OnToggle(_items.PanelID);
        _pause.OnToggle(_pause.PanelID);
    }

    private void GameInterfaceManager_ToggleMenuEvent(GameMenuType arg)
    {
        MenuPanel panel;
        switch (arg)
        {
            case GameMenuType.Items:
                _items.OnToggle(_items.PanelID);
                panel = _items;
                _unitsM.MenuOpened(panel.gameObject.activeSelf);
                break;
            case GameMenuType.Pause:
                _pause.OnToggle(_pause.PanelID);
                panel = _pause;
                _unitsM.MenuOpened(panel.gameObject.activeSelf);
                break;
        }        
    }



}


