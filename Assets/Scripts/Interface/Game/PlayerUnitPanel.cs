using Arcatech.Managers;
using Arcatech.Units;
using Arcatech.Units.Inputs;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.UI
{
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

        [SerializeField] IconContainersManager _icons;
        [SerializeField] BarsContainersManager _bars;


        [SerializeField, Space] protected float _barFillRateMult = 1f;

        //private void Shield_ComponentChangedStateToEvent(bool arg1, IStatsComponentForHandler arg2)
        //{
        //    if (arg1)
        //    {
        //        SHc = _shield.GetShieldStats[ShieldStatType.Shield];
        //    }
        //    _bars.ProcessContainer(SHc, DisplayValueType.Shield);
        //}



        public override void StartController()
        {
            _player = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit;
            _dodge = _player.GetInputs<InputsPlayer>().GetDodgeController;
            _weapons = _player.GetInputs<InputsPlayer>().GetWeaponController;
            _shield = _player.GetInputs<InputsPlayer>().GetShieldController;
            _combo = _player.GetInputs<InputsPlayer>().GetComboController;

            _bars.StartController();
            _icons.StartController();



            HPc = _player.GetStats[BaseStatType.Health];
            _bars.ProcessContainer(HPc, DisplayValueType.Health);
            _bars.ProcessContainer(_combo.ComboContainer,DisplayValueType.Combo);

            //_shield.ComponentChangedStateToEvent += Shield_ComponentChangedStateToEvent; // item was equipped


            //if (_shield.IsReady)
            //{                
            //    SHc = _shield.GetShieldStats[ShieldStatType.Shield];
            //    _icons.TrackItemIcon(_shield.CurrentlyEquippedItem);
            //    _bars.ProcessContainer(SHc, DisplayValueType.Shield);
            //}
            //if (_dodge.IsReady)
            //{
            //    _icons.TrackItemIcon(_dodge.CurrentlyEquippedItem); // TODO: Placeholder , no cooldown
            //}
            foreach (var weapon in _weapons.GetCurrentEquipped)
            {
                _icons.TrackItemIcon(weapon);
            }

        }


        public override void UpdateController(float delta)
        {
            _bars.UpdateController(delta);
            _icons.UpdateController(delta);
            //if (FillLerp < 1f) FillLerp += Mathf.Clamp01(delta * _barFillRateMult);

            //if (_weapons.IsReady)
            //{
            //    _ammoText.text = _weapons.GetUsesByType(EquipItemType.RangedWeap).ToString();
            //}
            //else _ammoText.text = "0";

            //if (HPc != null)
            //{
            //    _hpText.text = string.Concat(Math.Round(HPc.GetCurrent, 0), " / ", HPc.GetMax);
            //    ColorTexts(_hpText, HPc.GetMax, HPc.GetCurrent, minColorDefault, maxColorDefault);
            //    PrettyLerp(_hpBar, HPc);
            //}
            //if (HEc != null)
            //{
            //    _heText.text = string.Concat(Math.Round(HEc.GetCurrent, 0), " / ", HEc.GetMax);
            //    ColorTexts(_heText, HEc.GetMax, HEc.GetCurrent, minColorDefault, maxColorDefault);
            //    PrettyLerp(_heBar, HEc);
            //}
            //else _heText.text = "No combo value";
            //if (SHc != null)
            //{
            //    _spText.text = string.Concat(Math.Round(SHc.GetCurrent, 0), " / ", SHc.GetMax);
            //    ColorTexts(_spText, SHc.GetMax, SHc.GetCurrent, minColorDefault, maxColorDefault);
            //    PrettyLerp(_shBar, SHc);
            //}
            //else
            //{
            //    _spText.text = "Shield not equipped";
            //    _shBar.fillAmount = 0;
            //}

            //if (_dodge.IsReady)
            //{
            //    _dodgeText.text = _dodge.GetDodgeCharges().ToString();
            //}
            //else _dodgeText.text = "None";
        }

        public override void StopController()
        {
            _bars.StopController();
            _icons.StopController();
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






    }

}