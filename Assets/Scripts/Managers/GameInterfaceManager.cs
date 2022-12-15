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
    [SerializeField] private SelectedItemPanel _tgt;
    [SerializeField] private PlayerUnitPanel _player;
    private List<NPCUnit> _npcs;

    private UnitsManager _unitsM;

    private  void Start()
    {
        _unitsM = FindObjectOfType<UnitsManager>();
        if (_player == null) return;
        var p = _unitsM.GetPlayerUnit;

        _player.AssignItem(new SelectableUnitData(p,p.GetFullName,p.GetFaceCam),true);

        // TODO Maybe make a manager to handle all types of interactive items
        _npcs = new List<NPCUnit>(_unitsM.GetNPCs());

        BindAiming(true);

        _tgt.IsNeeded = false;
    }
    private void OnDisable()
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


