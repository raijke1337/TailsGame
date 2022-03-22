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

    private void Start()
    {
        if (_playerPanel == null) _playerPanel = FindObjectOfType<PlayerInfoPanel>();
        if (_tgtPanel == null) _tgtPanel = FindObjectOfType<TargetInfoPanel>();

        _playerPanel.RunSetup(_playerUnit);
        _tgtPanel.gameObject.SetActive(false);

        _playerUnit.GetController.TargetLockedEvent += (t) => AssignTarget(t);        
    }

    private void AssignTarget(IStatsAvailable unit)
    {
        _tgtPanel.RunSetup(unit);
        _tgtPanel.gameObject.SetActive(true);
        unit.UnitDiedEvent += HideTgtPanel;
    }
    private void HideTgtPanel(IStatsAvailable unit)
    {
        if (_tgtPanel.GetActiveUnit == unit)
            _tgtPanel.gameObject.SetActive(false);
    }
    // todo add smooth effects

}


