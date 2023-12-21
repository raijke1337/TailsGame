using Arcatech.Triggers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.Managers
{
    public class EventTriggersManager : LoadedManagerBase
    {
        [SerializeField] private List<BaseLevelEventTrigger> triggers = new List<BaseLevelEventTrigger>();
        private GameInterfaceManager _ui;
        private TriggersManager _trigs;

        #region managed
        public override void Initiate()
        {
            if (triggers != null) triggers.Clear();
            triggers = new List<BaseLevelEventTrigger>();
            _ui = GameManager.Instance.GetGameControllers.GameInterfaceManager;
            _trigs = GameManager.Instance.GetGameControllers.TriggersProjectilesManager;
        }

        public override void RunUpdate(float delta)
        {

        }

        public override void Stop()
        {
            foreach (var t in triggers.ToList()) { t.PlayerTagTriggerEvent -= OnEventActivated; }
        }
        #endregion

        public void RegisterEventTrigger(BaseLevelEventTrigger tr)
        {
            triggers.Add(tr);
            tr.PlayerTagTriggerEvent += OnEventActivated;
        }


        private void OnEventActivated(BaseLevelEventTrigger tr, bool isEnter)
        {
            if (tr is LevelEffectTrigger ef)
            {
                foreach (var e in ef.Triggers)
                {
                    _trigs.ServeTriggerApplication(e, null, GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit,isEnter);
                }
            }
            if (tr is LevelRewardTrigger rew)
            {
                GameManager.Instance.OnItemPickup(rew.Content);
                tr.PlayerTagTriggerEvent -= OnEventActivated;
                triggers.Remove(tr);
                Destroy(rew.gameObject);

            }
            if (tr is LevelTextTrigger txt)
            {
                _ui.UpdateGameText(txt.Text,isEnter);
            }
            if (tr is LevelCompleteTrigger comp)
            {
                GameManager.Instance.OnLevelComplete();
            }
            else
            {
                Debug.Log($"{tr.GetType()} has no assigned logic in {this}");
            }
        }


    }
}