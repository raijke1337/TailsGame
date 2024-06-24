using Arcatech.Puzzles;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Arcatech.Level;
namespace Arcatech.Managers
{
    public class LevelManager : LoadedManagerBase
    {
        [SerializeField] private List<BaseLevelEventTrigger> triggers = new List<BaseLevelEventTrigger>();
        private GameInterfaceManager _ui;
        private TriggersManager _trigs;

        protected List<LevelBlockDecorPrefabComponent> _levelBlocks;

        #region managed
        public override void Initiate()
        {
            if (triggers != null) triggers.Clear();
            triggers = new List<BaseLevelEventTrigger>();
            
            _ui = GameManager.Instance.GetGameControllers.GameInterfaceManager;
            _trigs = GameManager.Instance.GetGameControllers.TriggersProjectilesManager;


            _levelBlocks = FindObjectsOfType<LevelBlockDecorPrefabComponent>().ToList(); // find only starting "cubes"
            foreach (var t in _levelBlocks)
            {
                t.StartController();
            }

            foreach (var t in GameObject.FindObjectsOfType<BaseLevelEventTrigger>())
            {
                ManageTrigger(t, true);
            }
        }

        public override void RunUpdate(float delta)
        {
            foreach (var t in _levelBlocks)
            {
                t.UpdateController(delta);
            }
        }

        public override void Stop()
        {
            foreach (var t in triggers.ToList()) { ManageTrigger(t, false); }
            foreach (var t in _levelBlocks)
            {
                t.StopController();
            }
        }
        #endregion

        public void RegisterNewTrigger (BaseLevelEventTrigger tr, bool registering)
        {
            ManageTrigger(tr, registering);
        }

        private void ManageTrigger(BaseLevelEventTrigger tr,bool registering)
        {
            if (registering)
            {
                triggers.Add(tr);
                tr.TriggerHitUnitEvent += (u,e) => OnEventActivated(tr,u,e);
                if (tr is CheckConditionTrigger check)
                {
                    check.UpdatedConditionStateEvent += Check_UpdatedConditionStateEvent;
                }
            }
            else
            {
                triggers.Remove(tr);
                tr.TriggerHitUnitEvent -= (u, e) => OnEventActivated(tr, u, e);
                if (tr is CheckConditionTrigger check)
                {
                    check.UpdatedConditionStateEvent += Check_UpdatedConditionStateEvent;
                }
            }
        }

        private void Check_UpdatedConditionStateEvent(CheckConditionTrigger condition, bool currentStatee)
        {
            Debug.Log("NYI");
        }

        private void OnEventActivated(BaseLevelEventTrigger tr, BaseUnit u, bool isEnter)
        {
            if (tr is StaticEffectTrigger ef)
            {
                foreach (var e in ef.Triggers)
                {
                    _trigs.ServeTriggerApplication(new StatsEffect(e), null, GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit, isEnter);
                }
            }
            if (tr is LevelRewardTrigger rew)
            {
                //if (ShowDebug)
                //{
                //    Debug.Log($"Picked up {rew.Content}");
                //}
                u.AddItem(rew.Content,true);
                ManageTrigger(rew, false);

            }
            if (tr is LevelTextTrigger txt)
            {
                //if (ShowDebug)
                //{
                //    Debug.Log($"Level text trigger {txt}");
                //}
                _ui.UpdateGameText(txt.Text, isEnter);
            }
            if (tr is LevelCompleteTrigger comp)
            {
                //if (ShowDebug)
                //{
                //    Debug.Log($"");
                //}
                GameManager.Instance.OnLevelCompleteTrigger(comp.UnlocksLevel);
            }

            if (tr is CheckConditionTrigger check)
            {
                bool satisfied = check.GetCondition;
                check.UpdateControlledItems(satisfied);
                //if (ShowDebug)
                //{
                //    Debug.Log($"{}");
                //}
            }
        }
    }
}