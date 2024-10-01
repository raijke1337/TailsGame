using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.UI;
using Arcatech.Units;
using Arcatech.Units.Inputs;
using KBCore.Refs;
using System;
using System.Collections;
using UnityEngine;
namespace Arcatech.Managers
{
    public class GameInterfaceManager : MonoBehaviour, IManagedController
    {
        [SerializeField] private TargetPanel _tgtPan;
        [SerializeField] private PlayerUnitPanel _playerPan;
        [SerializeField] private GameTextComp _text;
        [SerializeField] private GameObject _ded;
        [SerializeField] private GameObject _pause;

        EventBinding<PlayerStatsChangedUIEvent> _statChangedBind;
        EventBinding<InventoryUpdateEvent> _inventoryChangedBind;
        EventBinding<PauseToggleEvent> _pauseToggleBind;
        EventBinding<PlayerTargetUpdateEvent> _targetUpdateBinding;
        

        #region managed
        public virtual void StartController()
        {


            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                _playerPan.gameObject.SetActive(true);
                
                _text.gameObject.SetActive(false);
                _text.DialogueCompleteEvent += OnDialogueCompletedInTextWindow;
                _ded.SetActive(false);

                _statChangedBind = new EventBinding<PlayerStatsChangedUIEvent>(UpdatePlayerBars);
                _inventoryChangedBind = new EventBinding<InventoryUpdateEvent>(UpdateIcons);
                _pauseToggleBind = new EventBinding<PauseToggleEvent>(ShowPauseMenu);
                _targetUpdateBinding = new EventBinding<PlayerTargetUpdateEvent>(OnTargetUpdate);

                EventBus<PlayerStatsChangedUIEvent>.Register(_statChangedBind);
                EventBus<InventoryUpdateEvent>.Register(_inventoryChangedBind);
                EventBus<PauseToggleEvent>.Register(_pauseToggleBind);
                EventBus<PlayerTargetUpdateEvent>.Register(_targetUpdateBinding);

            }
            else
            {
                _playerPan.gameObject.SetActive(false);
                _text.gameObject.SetActive(false);
                _text.DialogueCompleteEvent += OnDialogueCompletedInTextWindow; // dialogues also happen in scene levels
                _ded.SetActive(false);
                _tgtPan.gameObject.SetActive(false);
            }

        }


        public virtual void FixedControllerUpdate(float fixedDelta)
        {

        }

        public virtual void ControllerUpdate(float delta)
        {
            if (currentTgt != null)
            {
                _tgtPan.UpdateTargeted(true, currentTgt);
            }
        }
        private void OnDisable()
        {
            EventBus<PlayerStatsChangedUIEvent>.Deregister(_statChangedBind);
            EventBus<InventoryUpdateEvent>.Deregister(_inventoryChangedBind);
            EventBus<PauseToggleEvent>.Deregister(_pauseToggleBind);
            EventBus<PlayerTargetUpdateEvent>.Deregister(_targetUpdateBinding);
        }
        public virtual void StopController()
        {
            EventBus<PlayerStatsChangedUIEvent>.Deregister(_statChangedBind);
            EventBus<InventoryUpdateEvent>.Deregister(_inventoryChangedBind);
            EventBus<PauseToggleEvent>.Deregister(_pauseToggleBind);
            EventBus<PlayerTargetUpdateEvent>.Deregister(_targetUpdateBinding);
        }
        #endregion


        #region game dialogues and texts


        public void UpdateGameText(DialoguePart text, bool isShown)
        {

            if (isShown)
            {
                //_playerPan.LoadedDialogue(text, isShown);
                _text.gameObject.SetActive(isShown);
                _text.CurrentDialogue = text;
                if (text.Options.Count > 0)
                {
                    EventBus<PauseToggleEvent>.Raise(new PauseToggleEvent(isShown));
                }
            }    
            else
            {
                //_playerPan.LoadedDialogue(text, isShown);
                _text.gameObject.SetActive(isShown);
                EventBus<PauseToggleEvent>.Raise(new PauseToggleEvent(isShown));
            }

        }
        private void OnDialogueCompletedInTextWindow()
        {
            EventBus<PauseToggleEvent>.Raise(new PauseToggleEvent(false));
            _text.gameObject.SetActive(false);
        }


        #endregion

 
        #region UI events from event bus

        private void UpdatePlayerBars(PlayerStatsChangedUIEvent @event)
        {
            _playerPan.ShowBar(@event.StatType, @event.Container);
        }

        private void UpdateIcons(InventoryUpdateEvent obj)
        {
            if (obj.Unit is PlayerUnit)
            {
                _playerPan.ShowIcons(obj.Inventory);
            }
        }

        ITargetable currentTgt;

        void OnTargetUpdate(PlayerTargetUpdateEvent e)
        {
            if (e.Target is PlayerUnit) return; //dont show playuer
            if (e.IsPicked)
            {
                currentTgt = e.Target;  
                _tgtPan.UpdateTargeted(e.IsPicked, e.Target);
            }
            else
            {
                currentTgt = null;
            }
            _tgtPan.gameObject.SetActive(e.IsPicked);
        }

        #endregion


        #region menus

        public void ShowPauseMenu(PauseToggleEvent isPause)
        {
            // dont pause the game here
            _pause.SetActive(isPause.Value);
        }

        public void GameOver()
        {
            _ded.SetActive(true);
        }
        public void ToMain()
        {
            GameManager.Instance.OnReturnToMain();
        }
        public void OnRestart()
        {
            GameManager.Instance.RequestLoadSceneFromContainer(GameManager.Instance.GetCurrentLevelData);
        }
        public void ClickResume()
        {
            EventBus<PauseToggleEvent>.Raise(new PauseToggleEvent(false));
        }

        #endregion

    }


}
