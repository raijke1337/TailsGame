using Arcatech.Puzzles;
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
               u.GetUnitInventory.AddItem(rew.Content);
                ManageTrigger(rew, false);

            }
            if (tr is LevelTextTrigger txt)
            {
                _ui.UpdateGameText(txt.Text, isEnter);
            }
            if (tr is LevelCompleteTrigger comp)
            {
                GameManager.Instance.OnLevelCompleteTrigger(comp.UnlocksLevel);
            }
            if (tr is CheckItemTrigger item)
            {
                if (u.GetUnitInventory.HasItem(item.RequiredItem))
                {
                    item.DoPositiveAction();
                }
            }
            if (tr is CheckPuzzleTrigger puzzle)
            {

                HandlePuzzles(puzzle, isEnter);
            }
        }


        #region puzzles
        private CheckPuzzleTrigger _currentPuzzleTrigger;
        private BasePuzzleComponent _currentPuzzleWindow;

        private void HandlePuzzles(CheckPuzzleTrigger tr,bool enter)
        {
            if (tr.IsSolved) return;
            if (enter)
            {
                _currentPuzzleTrigger = tr;
                _currentPuzzleWindow = Instantiate(tr.PuzzlePrefab, _ui.transform);
                _currentPuzzleWindow.PuzzleResult += OnPuzzleResult;
                _ui.OnPauseRequest(true);
            }
            else
            {
                _currentPuzzleTrigger = null;
                _currentPuzzleWindow = null;
            }
        }
        private void OnPuzzleResult(bool ok)
        {
            _currentPuzzleWindow.PuzzleResult -= OnPuzzleResult;
            _ui.OnPauseRequest(false);
            Destroy(_currentPuzzleWindow.gameObject);
            if (ok)
            {
                _currentPuzzleTrigger.DoPositiveAction();
            }
            else
            {
                _currentPuzzleTrigger.DoNegativeAction();
            }
        }

        #endregion

    }
}