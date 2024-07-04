using Arcatech.EventBus;
using Arcatech.Items;
using Arcatech.Texts;
using Arcatech.UI;
using Arcatech.Units;
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

        EventBinding<StatChangedEvent> _statChangedBind;
        EventBinding<InventoryUpdateEvent> _inventoryChangedBind;


        #region managed
        public virtual void StartController()
        {


            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                _playerPan.gameObject.SetActive(true);
                
                _text.gameObject.SetActive(false);
                _text.DialogueCompleteEvent += OnDialogueCompletedInTextWindow;
                _ded.SetActive(false);

                _statChangedBind = new EventBinding<StatChangedEvent>(UpdatePlayerBars);
                _inventoryChangedBind = new EventBinding<InventoryUpdateEvent>(UpdateIcons);


                EventBus<StatChangedEvent>.Register(_statChangedBind);
                EventBus<InventoryUpdateEvent>.Register(_inventoryChangedBind);


            }
            else
            {
                _playerPan.gameObject.SetActive(false);
                _text.gameObject.SetActive(false);
                _text.DialogueCompleteEvent += OnDialogueCompletedInTextWindow; // dialogues also hap[pen in scene levels
                _ded.SetActive(false);
            }

        }


        public virtual void FixedControllerUpdate(float fixedDelta)
        {

        }

        public virtual void ControllerUpdate(float delta)
        {

        }
        private void OnDisable()
        {
            EventBus<StatChangedEvent>.Deregister(_statChangedBind);

            EventBus<InventoryUpdateEvent>.Deregister(_inventoryChangedBind);
        }
        public virtual void StopController()
        {
            EventBus<StatChangedEvent>.Deregister(_statChangedBind);
            EventBus<InventoryUpdateEvent>.Deregister(_inventoryChangedBind);
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
                    EventBus<PlayerPauseEvent>.Raise(new PlayerPauseEvent(isShown));
                }
            }    
            else
            {
                //_playerPan.LoadedDialogue(text, isShown);
                _text.gameObject.SetActive(isShown);
                EventBus<PlayerPauseEvent>.Raise(new PlayerPauseEvent(isShown));
            }

        }
        private void OnDialogueCompletedInTextWindow()
        {
            EventBus<PlayerPauseEvent>.Raise(new PlayerPauseEvent(false));
            _text.gameObject.SetActive(false);
        }




        #endregion

 
        #region UI events from event bus

        private void UpdatePlayerBars(StatChangedEvent @event)
        {
            _playerPan.ShowStat(@event.StatType, @event.Container);
        }

        private void UpdateIcons(InventoryUpdateEvent obj)
        {
            if (obj.Unit is PlayerUnit)
            {
                _playerPan.ShowIcons(obj.Inventory);
            }
        }
        #endregion


        #region menus

        public void OnPauseRequesShowPanelAndPause(bool isPause)
        {
            _pause.SetActive(isPause);
                    EventBus<PlayerPauseEvent>.Raise(new PlayerPauseEvent(isPause));
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

        #endregion

    }


}
