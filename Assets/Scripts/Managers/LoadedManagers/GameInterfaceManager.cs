using Arcatech.EventBus;
using Arcatech.Texts;
using Arcatech.UI;
using KBCore.Refs;
using System;
using System.Collections;
using UnityEngine;
namespace Arcatech.Managers
{
    public class GameInterfaceManager : LoadedManagerBase
    {
        [SerializeField,Child] private TargetPanel _tgtPan;
        [SerializeField, Child] private PlayerUnitPanel _playerPan;
        [SerializeField, Child] private GameTextComp _text;
        [SerializeField, Child] private GameObject _ded;
        [SerializeField, Child] private GameObject _pause;

        [Space, SerializeField] private CartoonFX.CFXR_ParticleText _textPrefab;


        EventBinding<DrawDamageEvent> _drawDamageBind;
        EventBinding<StatChangedEvent> _statChangedBind;


        #region managed
        public override void Initiate()
        {


            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                _playerPan.gameObject.SetActive(true);
                
                _text.gameObject.SetActive(false);
                _text.DialogueCompleteEvent += OnDialogueCompletedInTextWindow;
                _ded.SetActive(false);

                _drawDamageBind = new EventBinding<DrawDamageEvent>(DrawDamageNumber);
                EventBus<DrawDamageEvent>.Register(_drawDamageBind);

                _statChangedBind = new EventBinding<StatChangedEvent>(UpdatePlayerBars);
                EventBus<StatChangedEvent>.Register(_statChangedBind);

            }
            else
            {
                _playerPan.gameObject.SetActive(false);
                //_tgtPan.IsNeeded = false;
                _text.gameObject.SetActive(false);
                _text.DialogueCompleteEvent += OnDialogueCompletedInTextWindow; // dialogues also hap[pen in scene levels
                _ded.SetActive(false);
            }

        }



        public override void RunUpdate(float delta)
        {

        }
        private void OnDisable()
        {
            EventBus<DrawDamageEvent>.Deregister(_drawDamageBind);
            EventBus<StatChangedEvent>.Deregister(_statChangedBind);
        }
        public override void Stop()
        {
            
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
                    GameManager.Instance.OnPlayerPaused(true);
                }
            }    
            else
            {
                //_playerPan.LoadedDialogue(text, isShown);
                _text.gameObject.SetActive(isShown);
                GameManager.Instance.OnPlayerPaused(false);
            }

        }
        private void OnDialogueCompletedInTextWindow()
        {
            GameManager.Instance.OnPlayerPaused(false);
            _text.gameObject.SetActive(false);
        }




        #endregion

 
        #region UI events from event bus
        private void DrawDamageNumber (DrawDamageEvent data)
        {
            Debug.Log($"Draw damage number! {data.Unit.transform}"); 
        }

        private void UpdatePlayerBars(StatChangedEvent @event)
        {
            _playerPan.ShowStat(@event.StatType, @event.Container);
        }

        #endregion


        #region menus

        public void OnPauseRequesShowPanelAndPause(bool isPause)
        {
            _pause.SetActive(isPause);
            GameManager.Instance.OnPlayerPaused(isPause);
        }

        public void GameOver()
        {
            GameManager.Instance.OnPlayerPaused(true);
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
