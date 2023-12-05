using Arcatech.UI;
using System.Collections;
using System.Collections.Generic;
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

        private TextsManager _texts;
        private Coroutine _cor;

        #region managed
        public override void Initiate()
        {
            _texts = TextsManager.Instance;

            if (GameManager.Instance.GetCurrentLevelData.Type == LevelType.Game)
            {
                _player.IsNeeded = true;
                _player.StartController();
                _tgt.IsNeeded = false;
                _text.gameObject.SetActive(false);
                _ded.SetActive(false);

            }
            else
            {
                _player.IsNeeded = false;
                _tgt.IsNeeded = false;
                _text.gameObject.SetActive(false);
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

        public void UpdateGameText(string ID, bool isShown)
        {

            if (isShown)
            {
                _text.gameObject.SetActive(true);
                _text.SetText(_texts.GetContainerByID(ID));
            }
            else
            {
                _text.gameObject.SetActive(false);
            }
        }


        public void OnPlayerSelectedTargetable(BaseTargetableItem item, bool show)
        {
            if (show)
            {
                if (_cor != null) 
                { 
                    StopCoroutine(_cor); 
                }

                _tgt.IsNeeded = true;
                _tgt.AssignItem(item);
            }
            if (!show)
            {
                _cor = StartCoroutine(HideTargetPanel(_selPanelDisappearTimer));
            }
        }

        private IEnumerator HideTargetPanel(float time)
        {
            yield return new WaitForSeconds(time);
            _tgt.IsNeeded = false;
        }

        #region menus

        public void OnPauseRequest(bool isPause)
        {
            _pause.SetActive(true);
            GameManager.Instance.OnPlayerPaused(isPause);
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
            GameManager.Instance.RequestLevelLoad(GameManager.Instance.GetCurrentLevelData.LevelID);
        }

        #endregion

    }


}
