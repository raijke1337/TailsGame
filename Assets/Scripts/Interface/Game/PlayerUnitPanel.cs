using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitPanel : ManagedControllerBase
{

    private PlayerUnit _player;

    private DodgeController _dodge;
    private WeaponController _weapons;
    private ShieldController _shield;
    private ComboController _combo;


    private StatValueContainer SHc;
    private StatValueContainer HEc;
    private StatValueContainer HPc;

    protected List<StatValueContainer> _cont = new List<StatValueContainer>();


    [SerializeField] private Image _shBar;
    [SerializeField] private Image _heBar;
    [SerializeField] protected Image _hpBar;
    [SerializeField,Space] private TextMeshProUGUI _spText;
    [SerializeField] private TextMeshProUGUI _heText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _dodgeText;
    [SerializeField] private TextMeshProUGUI _ammoText;

    [SerializeField,Space] protected float _barFillRateMult = 1f;
    private float FillLerp = 0f;

    protected readonly Color minColorDefault = Color.red;
    protected readonly Color maxColorDefault = Color.black;
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

    public override void StartController()
    {
        _player = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit;
        _dodge = _player.GetInputs<InputsPlayer>().GetDodgeController;
        _weapons = _player.GetInputs<InputsPlayer>().GetWeaponController;
        _shield = _player.GetInputs<InputsPlayer>().GetShieldController;
        _combo = _player.GetInputs<InputsPlayer>().GetComboController;

        HPc = _player.GetStats[BaseStatType.Health];
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

    public override void UpdateController(float delta)
    {
        if (FillLerp < 1f) FillLerp += Mathf.Clamp01(delta * _barFillRateMult);

        if (_weapons.IsReady)
        {
            _ammoText.text = _weapons.GetUsesByType(EquipItemType.RangedWeap).ToString();
        }
        else _ammoText.text = "0";

        if (HPc != null)
        {
            _hpText.text = string.Concat(Math.Round(HPc.GetCurrent, 0), " / ", HPc.GetMax);
            ColorTexts(_hpText, HPc.GetMax, HPc.GetCurrent, minColorDefault, maxColorDefault);
            PrettyLerp(_hpBar, HPc);
        }
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
        else
        {
            _spText.text = "Shield not equipped";
            _shBar.fillAmount = 0;
        }

        if (_dodge.IsReady)
        {
            _dodgeText.text = _dodge.GetDodgeCharges().ToString();
        }
        else _dodgeText.text = "None";
    }

    public override void StopController()
    {
        if (HPc != null)
        {
            HPc.ValueChangedEvent -= ResetTicker;
        }
        if (_combo!= null && _combo.IsReady)
        {
            _combo.ComponentChangedStateToEvent -= Combo_ComponentChangedStateToEvent;
        }
        if (_shield!= null && _shield.IsReady)
        {
            _shield.ComponentChangedStateToEvent -= Shield_ComponentChangedStateToEvent;
        }
    }


    protected bool _act;
    public bool IsNeeded
    {
        get => _act;
        set
        {
            _act = value;
            gameObject.SetActive(value);
        }
    }




    protected virtual void ColorTexts(TextMeshProUGUI text, float max, float current, Color minC, Color maxC)
    {
        text.color = Color.Lerp(minC, maxC, current / max);
    }
    protected void PrettyLerp(Image bar, StatValueContainer cont)
    {
        bar.fillAmount = Mathf.Lerp(cont.GetLast / cont.GetMax, cont.GetCurrent / cont.GetMax, FillLerp);
    }

}

