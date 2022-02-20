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

    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _shBar;
    [SerializeField] private Image _heBar;


    [SerializeField] private Text _hpText;
    [SerializeField] private Text _spText;
    [SerializeField] private Text _heText;

    private UnitState playerState;

    private float _maxHP;
    private float _maxSH;
    private float _maxHE;

    private float _currentHP;
    private float _currentSH;
    private float _currentHE;

    private float _regHP;
    private float _regSH;
    private float _regHE;

    private async void Start()
    {
        await Task.Yield();
        playerState = _player.GetUnitState;

        _maxHP = playerState.GetAllCurrentStats[StatType.Health].GetCurrentValue;
        _maxSH = playerState.GetAllCurrentStats[StatType.Shield].GetCurrentValue;
        _maxHE = playerState.GetAllCurrentStats[StatType.Heat].GetCurrentValue;
    }

    private void LateUpdate()
    {
        if (playerState == null) return;
        UpdateCurrentValues();
        UpdateUI();
    }
    private void UpdateCurrentValues()
    {        
        _currentHP = playerState.CurrentHP;
        _currentSH = playerState.CurrentShield;
        _currentHE = playerState.CurrentHeat;
        _regHP = playerState.GetAllCurrentStats[StatType.HealthRegen].GetCurrentValue;
        _regSH = playerState.GetAllCurrentStats[StatType.ShieldRegen].GetCurrentValue;
        _regHE = playerState.GetAllCurrentStats[StatType.HeatRegen].GetCurrentValue;
    }
    private void UpdateUI()
    {
        _hpBar.fillAmount = _currentHP / _maxHP;
        _shBar.fillAmount = _currentSH / _maxSH;
        _heBar.fillAmount = _currentHE / _maxHE;

        _hpText.text = string.Concat(Math.Round(_currentHP, 0), " / ", _maxHP, " (", _regHP, " /s)");
        _spText.text = string.Concat(Math.Round(_currentSH, 0), " / ", _maxSH, " (", _regSH, " /s)");
        _heText.text = string.Concat(Math.Round(_currentHE, 0), " / ", _maxHE, " (", _regHE, " /s)");
    }
}

