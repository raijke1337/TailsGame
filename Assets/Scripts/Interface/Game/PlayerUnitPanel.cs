using Arcatech.Managers;
using Arcatech.Units;
using Arcatech.Units.Inputs;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.UI
{
    public class PlayerUnitPanel : PanelWithBarGeneric
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

            base.StartController(); // instantiate bars

            _icons.StartController();


            HPc = _player.GetStats[BaseStatType.Health];
            _bars.LoadValues(HPc, DisplayValueType.Health);
            HEc = _combo.GetAvailableCombo;
            _bars.LoadValues(HEc,DisplayValueType.Combo);

            //_shield.ComponentChangedStateToEvent += Shield_ComponentChangedStateToEvent; // item was equipped


            if (_shield.IsReady)
            {
                SHc = _shield.GetShieldStats[ShieldStatType.Shield];
                _bars.LoadValues(SHc, DisplayValueType.Shield);

                //foreach (var i in _shield.GetCurrentEquipped)
                //{
                //    _icons.TrackItemIcon(i);
                //}
            }

            //if (_dodge.IsReady)
            //{
            //    foreach (var i in _dodge.GetCurrentEquipped)
            //    {
            //        _icons.TrackItemIcon(i);
            //    }
            //}
            //foreach (var weapon in _weapons.GetCurrentEquipped)
            //{
            //    _icons.TrackItemIcon(weapon);
            //}

        }


        public override void UpdateController(float delta)
        {
            _bars.UpdateController(delta);
            _icons.UpdateController(delta);
        }

        public override void StopController()
        {
            _bars.StopController();
            _icons.StopController();
        }
    }

}