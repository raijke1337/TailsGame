using Arcatech.EventBus;
using Arcatech.Managers;
using Arcatech.Stats;
using Arcatech.Texts;
using Arcatech.Units;
using Arcatech.Units.Inputs;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.UI
{
    public class PlayerUnitPanel : PanelWithBarGeneric
    {

        private PlayerUnit _player;

        private StaminaController _combo;
        private SkillsController _skills;
        private StatValueContainer _energyCont;
        private StatValueContainer _staminaCont;
        private StatValueContainer _healthCont;

        protected List<StatValueContainer> _cont = new List<StatValueContainer>();

        [SerializeField] IconContainersManager _icons;




        public override void StartController()
        {
            return;
            _player = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit;

            //_healthCont = _player.GetInputs().AssessStat(BaseStatType.Health);
            //_energyCont = _player.GetInputs().AssessStat(BaseStatType.Energy);
            //_staminaCont = _player.GetInputs().AssessStat(BaseStatType.Energy);


           // _skills = _player.GetInputs().GetSkillsController;

            base.StartController(); // instantiate bars
            _icons.StartController();

            _bars.LoadValues(_healthCont, DisplayValueType.Health);
            _bars.LoadValues(_staminaCont, DisplayValueType.Stamina);
            _bars.LoadValues(_staminaCont, DisplayValueType.Energy);

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
            return;
            _bars.UpdateController(delta);
            _icons.UpdateController(delta);
        }

        public override void StopController()
        {
            return;
            _bars.StopController();
            _icons.StopController();
        }

        public void LoadedDialogue(DialoguePart d, bool isShown)
        {
            if (isShown && _player != null)
            {
                // null in scene levels
                _player.PlayerIsTalking(d);
            }
        }


    }

}