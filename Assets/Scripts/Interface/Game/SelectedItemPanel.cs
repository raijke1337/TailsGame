using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemPanel : MonoBehaviour
{
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

    private BasicSelectableItemData _data;
    [SerializeField] private TextMeshProUGUI _desctxt;





    [SerializeField] protected float _barFillRateMult = 1f;
    [SerializeField] protected float FillLerp = 0f;

    protected StatValueContainer HPc;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] protected Image _hpBar;
    [SerializeField] protected RectTransform _hpBG;

    protected readonly Color minColorDefault = Color.red;
    protected readonly Color maxColorDefault = Color.black;

    public virtual void AssignItem(BasicSelectableItemData data, bool isSelect)
    {
        _data = data;
        if (_desctxt != null) _desctxt.text = _data.TextString;
        IsNeeded = isSelect;


        if (_data is SelectableUnitData sud)
        {
            HPc = sud.Unit.GetStats()[BaseStatType.Health];
            _hpBG.gameObject.SetActive(isSelect);
        }
    }

    protected void Update()
    {
        if (!IsNeeded) return;
        UpdateBars();
    }

    protected virtual void UpdateBars()
    {
        if (HPc != null)
        {
            _hpText.text = string.Concat(Math.Round(HPc.GetCurrent, 0), " / ", HPc.GetMax);
            ColorTexts(_hpText, HPc.GetMax, HPc.GetCurrent, minColorDefault, maxColorDefault);
            PrettyLerp(_hpBar, HPc);
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

