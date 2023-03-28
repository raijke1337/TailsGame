using System.Collections;
using UnityEngine;

public class GameInterfaceManager : LoadedManagerBase
{
    [SerializeField] private EnemyUnitPanel _tgt;
    [SerializeField] private SelectableItemPanel _item;
    [SerializeField] private PlayerUnitPanel _player;
    [SerializeField] private GameTextComp _text;
    [SerializeField] private MenuPanel _ded;
    [SerializeField, Space] private float _selPanelDisappearTimer = 1f;


    private Coroutine _cor;

    #region managed
    public override void Initiate()
    {
        if (GameManager.Instance.GetCurrentLevelData.Type == LevelType.Game)
        {
            _player.IsNeeded = true;
            _player.StartController();
            _item.IsNeeded = false;
            _tgt.IsNeeded = false;
            _text.IsNeeded = false;
            _ded.OnToggle(false);

        }
        else
        {
            _player.IsNeeded = false;
            _tgt.IsNeeded = false;
            _item.IsNeeded = false;
            _text.IsNeeded = false;
            _ded.OnToggle(false);
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
            _text.IsNeeded = true;
            _text.SetText(TextsManager.Instance.GetContainerByID(ID));
        }
        else
        {
            _text.IsNeeded = false;
        }
    }

    private IEnumerator HidePanelTimer(float time, SelectableItemPanel panel)
    {
        yield return new WaitForSeconds(time);
        panel.IsNeeded = false;
    }

    public void UpdateSelected(SelectableItem item,bool show)
    {
        if (show)
        {
            if (_cor == null) StopCoroutine(_cor);
            switch (item.Type)
            {
                case SelectableItemType.None:
                    Debug.LogWarning($"{item} has no selectable type set!");
                    break;
                case SelectableItemType.Item:
                    _tgt.IsNeeded = false;
                    _item.IsNeeded = true;
                    _item.AssignItem(item);
                    break;
                case SelectableItemType.Unit:
                    _tgt.IsNeeded = true;
                    _item.IsNeeded = false;
                    _tgt.AssignItem(item);
                    break;
            }
        }
        if (!show)
        {
            if (_item.IsNeeded)
            {
                _cor = StartCoroutine(HidePanelTimer(_selPanelDisappearTimer, _item));
            }  
            else
            {
                _cor = StartCoroutine(HidePanelTimer(_selPanelDisappearTimer, _tgt));
            }
        }
    }

    #region menus
    public void GameOver()
    {
        _ded.OnToggle(true);
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


