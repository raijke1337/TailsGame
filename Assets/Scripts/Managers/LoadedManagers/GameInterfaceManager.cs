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

public class GameInterfaceManager : LoadedManagerBase
{
    [SerializeField] private SelectedItemPanel _tgt;
    [SerializeField] private PlayerUnitPanel _player;
    private List<NPCUnit> _npcs;

    public override void Initiate()
    {
        if (_player == null) return;
        var p = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit;

        _player.AssignItem(new SelectableUnitData(p, p.GetFullName, p.GetFaceCam), true);

        // TODO Maybe make a manager to handle all types of interactive items
        _npcs = new List<NPCUnit>(GameManager.Instance.GetGameControllers.UnitsManager.GetNPCs());

        BindAiming(true);

        _tgt.IsNeeded = false;
    }

    public override void RunUpdate(float delta)
    {

    }

    public override void Stop()
    {
        BindAiming(false);
    }

    private void BindAiming(bool isEnable)
    {
        if (_npcs.Count == 0) return;
        if (isEnable)
        {
            foreach (var n in _npcs)
            {
                n.MouseOverEvent += N_MouseOverEvent;
            }
        }
        else
        {
            foreach (var n in _npcs)
            {
                n.MouseOverEvent -= N_MouseOverEvent;
            }
        }
    }
    private void N_MouseOverEvent(BasicSelectableItemData arg1, bool arg2)
    {
        _tgt.AssignItem(arg1, arg2);
    }


}


