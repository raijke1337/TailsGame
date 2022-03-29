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

public class PlayerInfoPanel : BaseInfoPanel
{

    private DodgeController _dodge;
    private PlayerWeaponController _weapons;

    [SerializeField] private Image _shBar;
    [SerializeField] private Image _heBar;

    [SerializeField] private Text _spText;
    [SerializeField] private Text _heText;

    [SerializeField] private Text _dodgeText;
    [SerializeField] private Text _ammoText;



    private float _maxSH;
    private float _maxHE;
    private float _currentSH;
    private float _currentHE;

    private float _currentAmmo;
    private float _currentDodges;

    private PlayerUnit _player;


    public override void RunSetup(BaseUnit unit)
    {
        base.RunSetup(unit);
        _player = unit as PlayerUnit;

        _maxHE = _statdict[StatType.Heat].GetMax();
        _maxSH = _statdict[StatType.Shield].GetMax();

        _dodge = _player.GetController<PlayerUnitController>().GetDodgeController;
        _weapons = _player.GetController<PlayerUnitController>().GetWeaponController as PlayerWeaponController;
    }
    protected override void UpdateCurrentValues()
    {
        base.UpdateCurrentValues();
        _currentSH = _statdict[StatType.Shield].GetCurrent();
        _currentHE = _statdict[StatType.Heat].GetCurrent();
        _currentAmmo = _weapons.GetAmmoByType(WeaponType.Ranged);
        _currentDodges = _dodge.GetDodgeCharges();
    }
    protected override void UpdateUI()
    {
        base.UpdateUI();
        _spText.text = string.Concat(Math.Round(_currentSH, 0), " / ", _maxSH);
        _heText.text = string.Concat(Math.Round(_currentHE, 0), " / ", _maxHE);
        _dodgeText.text = _currentDodges.ToString();
        _ammoText.text = _currentAmmo.ToString();

        _shBar.fillAmount = _currentSH / _maxSH;
        _heBar.fillAmount = _currentHE / _maxHE;

        ColorTexts(_spText, _maxSH, _currentSH);
        ColorTexts(_heText, _maxHE, _currentHE);
    }
}

