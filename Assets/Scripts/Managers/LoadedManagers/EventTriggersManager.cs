using Arcatech.Triggers;
using Arcatech.Units;
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

            foreach (var t in GameObject.FindObjectsOfType<BaseLevelEventTrigger>())
            {
                ManageTrigger(t,true);
            }

        }

        public override void RunUpdate(float delta)
        {

        }

        public override void Stop()
        {
            foreach (var t in triggers.ToList()) { ManageTrigger(t, false); }
        }
        #endregion

        private void ManageTrigger(BaseLevelEventTrigger tr,bool registering)
        {
            if (registering)
            {
                triggers.Add(tr);
                tr.TriggerHitUnitEvent += (u,e) => OnEventActivated(tr,u,e);
            }
            else
            {
                triggers.Remove(tr);
                tr.TriggerHitUnitEvent -= (u, e) => OnEventActivated(tr, u, e);
            }
        }


        private void OnEventActivated(BaseLevelEventTrigger tr, BaseUnit u, bool isEnter)
        {
            if (tr is LevelEffectTrigger ef)
            {
                foreach (var e in ef.Triggers)
                {
                    _trigs.ServeTriggerApplication(e, null, GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit, isEnter);
                }
            }
            if (tr is LevelRewardTrigger rew)
            {
                GameManager.Instance.OnItemPickup(rew.Content);
                ManageTrigger(rew, false);

            }
            if (tr is LevelTextTrigger txt)
            {
                _ui.UpdateGameText(txt.Text, isEnter);
            }
            if (tr is LevelCompleteTrigger comp)
            {
                GameManager.Instance.OnLevelComplete();
            }
        }


    }
}