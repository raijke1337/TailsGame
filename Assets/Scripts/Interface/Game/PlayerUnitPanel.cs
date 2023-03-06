using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitPanel : SelectedItemPanel
{

    private PlayerUnit _player;

    private DodgeController _dodge;
    private WeaponController _weapons;
    private ShieldController _shield;
    private ComboController _combo;


    private StatValueContainer SHc;
    private StatValueContainer HEc;


    protected List<StatValueContainer> _cont = new List<StatValueContainer>();


    [SerializeField] private Image _shBar;
    [SerializeField] private Image _heBar;


    [SerializeField] private TextMeshProUGUI _spText;
    [SerializeField] private TextMeshProUGUI _heText;
    [SerializeField] private TextMeshProUGUI _dodgeText;
    [SerializeField] private TextMeshProUGUI _ammoText;


    public override void AssignItem(BasicSelectableItemData data, bool isSelect)
    {
        _player = (data as SelectableUnitData).Unit as PlayerUnit;
        RunSetup();
        base.AssignItem(data, isSelect);

    }
    protected void RunSetup()
    {

        _dodge = _player.GetInputs<InputsPlayer>().GetDodgeController;
        _weapons = _player.GetInputs<InputsPlayer>().GetWeaponController;
        _shield = _player.GetInputs<InputsPlayer>().GetShieldController;
        _combo = _player.GetInputs<InputsPlayer>().GetComboController;

        HPc = _player.GetStats()[BaseStatType.Health];
        HPc.ValueChangedEvent += ResetTicker;

        if (_combo.IsReady)
        {
            HEc = _combo.ComboContainer;
            _combo.ComponentChangedStateToEvent += Combo_ComponentChangedStateToEvent;
        }
        if (_shield.IsReady)
        {
            _shield.ComponentChangedStateToEvent += Shield_ComponentChangedStateToEvent;
            SHc = _shield.GetShieldStats[ShieldStatType.Shield];
        }

    }

    private void Combo_ComponentChangedStateToEvent(bool arg1, IStatsComponentForHandler arg2)
    {
        if (arg1)
            HEc = _combo.ComboContainer;
    }

    private void Shield_ComponentChangedStateToEvent(bool arg1, IStatsComponentForHandler arg2)
    {
        if (arg1)
            SHc = _shield.GetShieldStats[ShieldStatType.Shield];
    }

    protected void ResetTicker(float arg1, float arg2)
    {
        FillLerp = 0f;
    }



    protected override void UpdateBars()
    {
        if (FillLerp < 1f) FillLerp += Mathf.Clamp01(Time.deltaTime * _barFillRateMult);

        if (_weapons.IsReady)
        {
            _ammoText.text = _weapons.GetAmmoByType(EquipItemType.RangedWeap).ToString();
        }
        else _ammoText.text = "0";

        if (HEc != null)
        {
            _heText.text = string.Concat(Math.Round(HEc.GetCurrent, 0), " / ", HEc.GetMax);
            ColorTexts(_heText, HEc.GetMax, HEc.GetCurrent, minColorDefault, maxColorDefault);
            PrettyLerp(_heBar, HEc);
        }
        else _heText.text = "No combo value";
        if (SHc != null)
        {
            _spText.text = string.Concat(Math.Round(SHc.GetCurrent, 0), " / ", SHc.GetMax);
            ColorTexts(_spText, SHc.GetMax, SHc.GetCurrent, minColorDefault, maxColorDefault);
            PrettyLerp(_shBar, SHc);
        }
        else _spText.text = "Shield not equipped";

        if (_dodge.IsReady)
        {
            _dodgeText.text = _dodge.GetDodgeCharges().ToString();
        }
        else _dodgeText.text = "None";

        base.UpdateBars();
    }



}

