using Arcatech.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Managers
{
    public class GameInterfaceManager : LoadedManagerBase
    {
        [SerializeField] private EnemyUnitPanel _tgt;
        [SerializeField] private PlayerUnitPanel _player;
        [SerializeField] private GameTextComp _text;
        [SerializeField] private GameObject _ded;
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
            if (_tgt.IsNeeded) _tgt.UpdateBars(delta);
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
        private IEnumerator HidePanelTimer(float time, GameObject panel)
        {
            yield return new WaitForSeconds(time);
            panel.SetActive(false);
        }

        public void UpdateSelected(SelectableItem item, bool show)
        {
            if (show)
            {
                if (_cor == null) StopCoroutine(_cor);
                switch (item.Type)
                {
                    default:
                        break;
                    case SelectableItemType.Item:
                        _text.gameObject.SetActive(true);
                        _text.SetText(_texts.GetContainerByID(item.GetTextID));
                        break;
                    case SelectableItemType.Unit:
                        _tgt.IsNeeded = true;
                        _tgt.AssignItem(item);
                        break;
                }
            }
            if (!show)
            {
                if (_text.isActiveAndEnabled)
                {
                    _cor = StartCoroutine(HidePanelTimer(_selPanelDisappearTimer, _text.gameObject));
                }
                else
                {
                    _cor = StartCoroutine(HidePanelTimer(_selPanelDisappearTimer, _tgt.gameObject));
                }
            }
        }

        #region menus
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
