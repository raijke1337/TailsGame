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

public class GameInterfaceManager : MonoBehaviour
{
    [SerializeField] private BaseInfoPanel _playerPanel;
    [SerializeField] private BaseInfoPanel _tgtPanel;

    [Inject]
    private PlayerUnit _playerUnit;
    private BaseUnit _targetUnit;

    private void Start()
    {
        if (_playerPanel == null) _playerPanel = FindObjectOfType<PlayerInfoPanel>();
        if (_tgtPanel == null) _tgtPanel = FindObjectOfType<TargetInfoPanel>();

        _playerPanel.RunSetup(_playerUnit);
        _tgtPanel.gameObject.SetActive(false);

        _playerUnit.GetController<PlayerUnitController>().TargetLockedEvent += (t) => AssignTarget(t);        
    }

    private void AssignTarget(BaseUnit unit)
    {
        if (_targetUnit != unit && _targetUnit != null)
        {
            _targetUnit.ToggleCamera(false);
        }
        _targetUnit = unit;


        _tgtPanel.RunSetup(_targetUnit);
        _tgtPanel.gameObject.SetActive(true);

        _targetUnit.ToggleCamera(true);

        _targetUnit.UnitDiedEvent += HideTgtPanel;
    }

    // potential memory leak?
    private void HideTgtPanel(BaseUnit unit)
    {
        _targetUnit.ToggleCamera(false);

        if (_targetUnit == unit)
        {
            _tgtPanel.gameObject.SetActive(false);
        }
    }
    // todo add smooth effects

}


