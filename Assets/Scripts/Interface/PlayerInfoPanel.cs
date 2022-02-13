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
using Zenject;
using System.Threading.Tasks;

public class PlayerInfoPanel : MonoBehaviour
{
    [Inject]
    private PlayerUnit _player;

    [SerializeField] private Image _hpB;
    [SerializeField] private Image _shB;
    [SerializeField] private Image _heB;



    private float _maxHP;
    private float _maxSH;
    private float _maxHE;

    private float _currentHP;
    private float _currentDH;
    private float _currentHE;

    private float _regHP;
    private float _regSH;
    private float _regHE;

    private async void Start()
    {
        await Task.Yield();

        var max = _player.GetUnitState.GetAllCurrentStats;
        _maxHP = max[StatType.Health].Range.Max;
        _maxSH = max[StatType.Shield].Range.Max;
        _maxHE = max[StatType.Heat].Range.Max;
        _regHP = max[StatType.HealthRegen].GetCurrentValue;
        _regSH = max[StatType.ShieldRegen].GetCurrentValue;
        _regHE = max[StatType.HeatRegen].GetCurrentValue;
        _currentHP = max[StatType.Health].GetCurrentValue;
        _currentDH = max[StatType.Shield].GetCurrentValue;
        _currentHE = max[StatType.Heat].GetCurrentValue;
    }




}

