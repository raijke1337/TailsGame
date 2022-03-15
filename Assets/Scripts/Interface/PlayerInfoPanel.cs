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
using RotaryHeart.Lib.SerializableDictionary;

public class PlayerInfoPanel : MonoBehaviour
{
    [Inject]
    private PlayerUnit _player;

    private IReadOnlyDictionary<StatType, StatValueContainer> _statdict;

    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _shBar;
    [SerializeField] private Image _heBar;


    [SerializeField] private Text _hpText;
    [SerializeField] private Text _spText;
    [SerializeField] private Text _heText;


    [SerializeField] private Text _dodgeText;

    private float _maxHP;
    private float _maxSH;
    private float _maxHE;

    private float _currentHP;
    private float _currentSH;
    private float _currentHE;
    private float _currentD;



    private float _regHP;
    private float _regSH;
    private float _regHE;

    private void Start()
    {

        _statdict = _player.GetStats;

        _maxHP = _statdict[StatType.Health].GetMax();
        _maxHE = _statdict[StatType.Heat].GetMax();
        _maxSH = _statdict[StatType.Shield].GetMax();
    }

    private void LateUpdate()
    {
        if (_statdict == null | _player == null) return;
        UpdateCurrentValues();
        UpdateUI();
    }
    private void UpdateCurrentValues()
    {
        _currentHP = _statdict[StatType.Health].GetCurrent();
        _currentSH = _statdict[StatType.Shield].GetCurrent();
        //_currentD = _player.GetDashStats[DashStatType.DashCharges].GetCurrentValue;
        _regSH = _statdict[StatType.ShieldRegen].GetCurrent();
        _regHP = _statdict[StatType.HealthRegen].GetCurrent();

        _currentHE = _statdict[StatType.Heat].GetCurrent();
        _regHE = _statdict[StatType.HeatRegen].GetCurrent();
    }
    private void UpdateUI()
    {
        _hpBar.fillAmount = _currentHP / _maxHP;
        _shBar.fillAmount = _currentSH / _maxSH;
        _heBar.fillAmount = _currentHE / _maxHE;

        _dodgeText.text = _currentD.ToString();

        _hpText.text = string.Concat(Math.Round(_currentHP, 0), " / ", _maxHP, " (", _regHP, " /s)");
        _spText.text = string.Concat(Math.Round(_currentSH, 0), " / ", _maxSH, " (", _regSH, " /s)");
        _heText.text = string.Concat(Math.Round(_currentHE, 0), " / ", _maxHE, " (", _regHE, " /s)");
    }
}

