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

public class PlayerUnitPanel : BaseUnitPanel
{

    private PlayerUnit _player;

    private DodgeController _dodge;
    private WeaponController _weapons;
    private ShieldController _shield;

    private StatValueContainer HPc;
    private StatValueContainer SHc;
    private StatValueContainer HEc;


    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _shBar;
    [SerializeField] private Image _heBar;

    [SerializeField] private Text _hpText;
    [SerializeField] private Text _spText;
    [SerializeField] private Text _heText;

    [SerializeField] private Text _dodgeText;
    [SerializeField] private Text _ammoText;


    private float _maxHP;
    private float _currentHP;
    private float _maxSH;
    private float _maxHE;
    private float _currentSH;
    private float _currentHE;

    private float _currentAmmo;
    private float _currentDodges;

    public override void AssignItem(BaseUnit item, bool isSelect)
    {
        _player = item as PlayerUnit;
        base.AssignItem(item, isSelect);
    }
    protected override void RunSetup()
    {
        _dodge = _player.GetInputs<InputsPlayer>().GetDodgeController;
        _weapons = _player.GetInputs<InputsPlayer>().GetWeaponController;
        _shield = _player.GetInputs<InputsPlayer>().GetShieldController;

        HPc = _player.GetStats()[StatType.Health];
        HEc = _player.GetStats()[StatType.Heat];
        SHc = _shield.GetShieldStats()[ShieldStatType.Shield];
    }


    protected override void UpdateValues()
    {
        _maxHP = HPc.GetMax();
        _currentHP = HPc.GetCurrent();
        _maxSH = SHc.GetMax();
        _currentSH = SHc.GetCurrent();
        _maxHE = HEc.GetMax();
        _currentHE = HEc.GetCurrent();
        _currentAmmo = _weapons.GetAmmoByType(WeaponType.Ranged);
        _currentDodges = _dodge.GetDodgeCharges();
    }

    protected override void UpdateDisplay()
    {


        _hpText.text = string.Concat(Math.Round(_currentHP, 0), " / ", _maxHP);
        _spText.text = string.Concat(Math.Round(_currentSH, 0), " / ", _maxSH);
        _heText.text = string.Concat(Math.Round(_currentHE, 0), " / ", _maxHE);

        _dodgeText.text = _currentDodges.ToString();
        _ammoText.text = _currentAmmo.ToString();


        _hpBar.fillAmount = _currentHP / _maxHP;
        _shBar.fillAmount = _currentSH / _maxSH;
        _heBar.fillAmount = _currentHE / _maxHE;

        ColorTexts(_hpText, _maxHP, _currentHP,minColorDefault,maxColorDefault);
        ColorTexts(_spText, _maxSH, _currentSH,minColorDefault, maxColorDefault);
        ColorTexts(_heText, _maxHE, _currentHE,minColorDefault, maxColorDefault);

    }

}

