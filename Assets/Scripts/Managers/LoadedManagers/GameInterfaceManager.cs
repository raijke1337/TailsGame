using Arcatech.Texts;
using Arcatech.UI;
using System.Collections;
using UnityEngine;
namespace Arcatech.Managers
{
    public class GameInterfaceManager : LoadedManagerBase
    {
        [SerializeField] private TargetPanel _tgt;
        [SerializeField] private PlayerUnitPanel _player;
        [SerializeField] private GameTextComp _text;
        [SerializeField] private GameObject _ded;
        [SerializeField] private GameObject _pause;
        [SerializeField, Space] private float _selPanelDisappearTimer = 1f;

        private Coroutine _cor;

        #region managed
        public override void Initiate()
        {

            if (GameManager.Instance.GetCurrentLevelData.LevelType == LevelType.Game)
            {
                _player.IsNeeded = true;
                _player.StartController();
                _tgt.IsNeeded = false;
                _text.gameObject.SetActive(false);
                _text.DialogueCompleteEvent += OnDialogueCompletedInTextWindow;
                _ded.SetActive(false);

            }
            else
            {
                _player.IsNeeded = false;
                _tgt.IsNeeded = false;
                _text.gameObject.SetActive(false);
                _text.DialogueCompleteEvent += OnDialogueCompletedInTextWindow; // dialogues also hap[pen in scene levels
                _ded.SetActive(false);
            }

        }



        public override void RunUpdate(float delta)
        {
            _player.UpdateController(delta);
            if (_tgt.IsNeeded) _tgt.UpdateController(delta);
        }

        public override void Stop()
        {
            _player.StopController();
        }
        #endregion


        #region game dialogues and texts


        public void UpdateGameText(DialoguePart text, bool isShown)
        {

            if (isShown)
            {
                _player.LoadedDialogue(text, isShown);
                _text.gameObject.SetActive(isShown);
                _text.CurrentDialogue = text;
                if (text.Options.Count > 0)
                {
                    GameManager.Instance.OnPlayerPaused(true);
                }
            }    
            else
            {
                _player.LoadedDialogue(text, isShown);
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

        #region target panel
        public void OnPlayerSelectedTargetable(BaseTargetableItem item, bool show)
        {
            // comm ented because I dont like the panel
            //if (show)
            //{
            //    if (_cor != null)
            //    {
            //        StopAllCoroutines();
            //    }

            //    paneltimer = _selPanelDisappearTimer;
            //    _tgt.IsNeeded = true;
            //    _tgt.AssignItem(item);
            //}
            //if (!show)
            //{
            //    paneltimer = _selPanelDisappearTimer;
            //    _cor = StartCoroutine(HidePanel(_tgt));
            //}
        }
        private float paneltimer;
        private IEnumerator HidePanel(PanelWithBarGeneric item)
        {
            while (paneltimer > 0)
            {
                paneltimer -= Time.deltaTime;
                yield return null;
            }
            item.IsNeeded = false;
            yield return null;
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
