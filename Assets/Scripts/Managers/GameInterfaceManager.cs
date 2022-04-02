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
    private UnitsManager _unitsM;

    private async void Start()
    {
        await Task.Yield();

        _unitsM = FindObjectOfType<UnitsManager>();
        _player.AssignItem(_unitsM.GetPlayerUnit(),true);
        foreach (var npc in _unitsM.GetNPCs())
        {
            npc.SelectionEvent += NPCmousedEventForUI;
        }
    }

    private void NPCmousedEventForUI(InteractiveItem item, bool isSelected)
    {
        _tgt.AssignItem(item as NPCUnit, isSelected);
    }
}


