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
    private ComboController _combo;

    private StatValueContainer HPc;
    private StatValueContainer SHc;
    private StatValueContainer HEc;


    protected List<StatValueContainer> _cont = new List<StatValueContainer>();
    [SerializeField] protected float FillLerp = 0f;


    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _shBar;
    [SerializeField] private Image _heBar;

    [SerializeField] private Text _hpText;
    [SerializeField] private Text _spText;
    [SerializeField] private Text _heText;
    [SerializeField] private Text _dodgeText;
    [SerializeField] private Text _ammoText;


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
        _combo = _player.GetInputs<InputsPlayer>().GetComboController;

        HPc = _player.GetStats()[BaseStatType.Health];
        HEc = _combo.ComboContainer;
        SHc = _shield.GetShieldStats[ShieldStatType.Shield];

        HPc.ValueChangedEvent += ResetTicker; // only hp because other stats regen 
    }

    protected void ResetTicker(float arg1, float arg2)
    {
        FillLerp = 0f;
    }



    protected override void UpdatePanel()
    {
        if (FillLerp < 1f) FillLerp += Mathf.Clamp01(Time.deltaTime * _barFillRateMult);

        _currentAmmo = _weapons.GetAmmoByType(WeaponType.Ranged);
        _currentDodges = _dodge.GetDodgeCharges();


        _hpText.text = string.Concat(Math.Round(HPc.GetCurrent(), 0), " / ", HPc.GetMax());
        _spText.text = string.Concat(Math.Round(SHc.GetCurrent(), 0), " / ", SHc.GetMax());
        _heText.text = string.Concat(Math.Round(HEc.GetCurrent(), 0), " / ", HEc.GetMax());

        _dodgeText.text = _currentDodges.ToString();
        _ammoText.text = _currentAmmo.ToString();

        ColorTexts(_hpText, HPc.GetMax(), HPc.GetCurrent(), minColorDefault, maxColorDefault);
        ColorTexts(_spText, SHc.GetMax(), SHc.GetCurrent(), minColorDefault, maxColorDefault);
        ColorTexts(_heText, HEc.GetMax(), HEc.GetCurrent(), minColorDefault, maxColorDefault);

        PrettyLerp();

    }

    protected void PrettyLerp()
    {
        _hpBar.fillAmount = Mathf.Lerp(HPc.GetLast() / HPc.GetMax(), HPc.GetCurrent() / HPc.GetMax(), FillLerp);
        _shBar.fillAmount = Mathf.Lerp(SHc.GetLast() / SHc.GetMax(), SHc.GetCurrent() / SHc.GetMax(), FillLerp);
        _heBar.fillAmount = Mathf.Lerp(HEc.GetLast() / HEc.GetMax(), HEc.GetCurrent() / HEc.GetMax(), FillLerp);

    }


}

