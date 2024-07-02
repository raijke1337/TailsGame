using Arcatech.Puzzles;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Arcatech.Level;
using Arcatech.EventBus;
using Arcatech.Items;

namespace Arcatech.Managers
{
    public class LevelManager : MonoBehaviour, IManagedController
    {
        [SerializeField] private List<BaseLevelEventTrigger> triggers = new List<BaseLevelEventTrigger>();
        private GameInterfaceManager _ui;
        private TriggersManager _trigs;

        protected List<LevelBlockDecorPrefabComponent> _levelBlocks;

        #region managed
        public virtual void StartController()
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

        public virtual void ControllerUpdate(float delta)
        {
            foreach (var t in _levelBlocks)
            {
                t.ControllerUpdate(delta);
            }
        }
        public virtual void FixedControllerUpdate(float fixedDelta)
        {

        }

        public virtual void StopController()
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
        ///
        /// Massive todo
        ///
        private void ManageTrigger(BaseLevelEventTrigger tr,bool registering)
        {
            if (registering)
            {
                triggers.Add(tr);
                //tr.TriggerHitUnitEvent += (u,e) => OnEventActivated(tr,u,e);
                if (tr is CheckConditionTrigger check)
                {
                    check.UpdatedConditionStateEvent += Check_UpdatedConditionStateEvent;
                }
            }
            else
            {
                triggers.Remove(tr);
               // tr.TriggerHitUnitEvent -= (u, e) => OnEventActivated(tr, u, e);
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

        private void OnEventActivated(BaseLevelEventTrigger tr, DummyUnit u, bool isEnter)
        {

            if (tr is LevelRewardTrigger rew)
            {
                //if (DebugMessage)
                //{
                //    Debug.Log($"Picked up {rew.Content}");
                //}
                //u.AddItem(rew.Content,true);

                ManageTrigger(rew, false);

            }
            if (tr is LevelTextTrigger txt)
            {
                //if (DebugMessage)
                //{
                //    Debug.Log($"Level text trigger {txt}");
                //}
                _ui.UpdateGameText(txt.Text, isEnter);
            }
            if (tr is LevelCompleteTrigger comp)
            {

                EventBus<LevelCompletedEvent>.Raise(new LevelCompletedEvent(comp.ThisLevel));
            }

            if (tr is CheckConditionTrigger check)
            {
                bool satisfied = check.GetCondition;
                check.UpdateControlledItems(satisfied);
                //if (DebugMessage)
                //{
                //    Debug.Log($"{}");
                //}
            }
        }
    }
}