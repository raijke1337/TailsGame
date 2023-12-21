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

        private ShieldController _shield;
        private ComboController _combo;
        private SkillsController _skills;


        private StatValueContainer SHc;
        private StatValueContainer HEc;
        private StatValueContainer HPc;

        protected List<StatValueContainer> _cont = new List<StatValueContainer>();

        [SerializeField] IconContainersManager _icons;

        public override void StartController()
        {

            _player = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit;
            _shield = _player.GetInputs<InputsPlayer>().GetShieldController;
            _combo = _player.GetInputs<InputsPlayer>().GetComboController;

            _skills = _player.GetInputs().GetSkillsController;

            base.StartController(); // instantiate bars

            _icons.StartController();


            HPc = _player.GetStats[BaseStatType.Health];
            _bars.LoadValues(HPc, DisplayValueType.Health);
            HEc = _combo.GetAvailableCombo;
            _bars.LoadValues(HEc, DisplayValueType.Combo);

            if (_shield.IsReady)
            {
                SHc = _shield.GetShieldStats[ShieldStatType.Shield];
                _bars.LoadValues(SHc, DisplayValueType.Shield);
            }
            if (_skills.IsReady)
            {
                foreach (var sk in _skills.GetControlData())
                {
                    _icons.TrackSkillIcon(sk);
                }
            }

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