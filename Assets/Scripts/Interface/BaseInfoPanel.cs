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

public abstract class BaseInfoPanel : MonoBehaviour
{
    protected IUnitForTargetPanel _unit;
    protected IReadOnlyDictionary<StatType, StatValueContainer> _statdict;
    public IUnitForTargetPanel GetActiveUnit => _unit;

    public virtual void RunSetup(IUnitForTargetPanel unit = null)
    {
        if (unit == null) return;
        _unit = unit;
        _statdict = _unit.GetStats();

        _maxHP = _statdict[StatType.Health].GetMax();

        if (_nameText == null) return;
        // player info has no name text
        _nameText.text = _unit.GetName();
    }

    [SerializeField] protected Image _hpBar;
    [SerializeField] protected Text _hpText;
    [SerializeField] protected Text _nameText;

    protected float _maxHP;
    protected float _currentHP;

    protected virtual void LateUpdate()
    {
        if (_statdict == null | _unit == null) return;
        UpdateCurrentValues();
        UpdateUI();
    }


    protected virtual void UpdateCurrentValues()
    {
        _currentHP = _statdict[StatType.Health].GetCurrent();
    }
    protected virtual void UpdateUI()
    {
        _hpBar.fillAmount = _currentHP / _maxHP;
        _hpText.text = string.Concat(Math.Round(_currentHP, 0), " / ", _maxHP);
    }

}

