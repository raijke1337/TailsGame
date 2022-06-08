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

public class TargetUnitPanel : BaseUnitPanel
{
    private NPCUnit _unit;

    [SerializeField] protected Image _hpBar;
    [SerializeField] protected Text _hpText;
    [SerializeField] protected Text _nameText;

    protected StatValueContainer HPc;

    protected float _maxHP;
    protected float _currentHP;


    public override void AssignItem(BaseUnit item, bool isSelect)
    {
        _unit = item as NPCUnit;
        base.AssignItem(item, isSelect);
    }
    protected override void RunSetup()
    {
        HPc = _unit.GetStats()[BaseStatType.Health];
        _nameText.text = _unit.GetFullName();
    }

    protected override void UpdatePanel()
    {
        _maxHP = HPc.GetMax;
        _currentHP = HPc.GetCurrent;
        _hpText.text = string.Concat(Math.Round(_currentHP, 0), " / ", _maxHP);
        _hpBar.fillAmount = _currentHP / _maxHP;
        ColorTexts(_hpText, _maxHP, _currentHP, minColorDefault, maxColorDefault);
    }

}

