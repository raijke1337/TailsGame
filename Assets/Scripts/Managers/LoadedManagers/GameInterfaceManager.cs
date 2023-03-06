using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameInterfaceManager : LoadedManagerBase
{
    [SerializeField] private SelectedItemPanel _tgt;
    [SerializeField] private PlayerUnitPanel _player;
    [SerializeField] private GameTextComp _text;
    [SerializeField] private MenuPanel _ded;
    private List<NPCUnit> _npcs;

    public override void Initiate()
    {
        if (GameManager.Instance.GetCurrentLevelData.Type == LevelType.Game)
        {
            if (_player == null) return;
            var p = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit;

            _player.AssignItem(new SelectableUnitData(p, p.GetFullName, p.GetFaceCam), true);

            // TODO Maybe make a manager to handle all types of interactive items
            _npcs = new List<NPCUnit>(GameManager.Instance.GetGameControllers.UnitsManager.GetNPCs());

            BindAiming(true);
            _tgt.IsNeeded = false;
            _text.IsShown = false;
            _ded.OnToggle(false);
        }
        else
        {
            _player.IsNeeded = false;
            _tgt.IsNeeded = false;
            _text.IsShown = false;
            _ded.OnToggle(false);
        }

    }

    public void UpdateGameText(string ID, bool isShown)
    {
        if (isShown)
        {
            _text.IsShown = true;
            _text.SetText(TextsManager.Instance.GetContainerByID(ID));
        }
        else
        {
            _text.IsShown = false;
        }
    }

    public override void RunUpdate(float delta)
    {
        // TODO update bars etc here
    }

    public override void Stop()
    {
        BindAiming(false);
    }

    private void BindAiming(bool isEnable)
    {
        if (_npcs == null || _npcs.Count == 0) return;
        if (isEnable)
        {
            foreach (var n in _npcs)
            {
                n.MouseOverEvent += N_MouseOverEvent;
            }
        }
        else
        {
            foreach (var n in _npcs)
            {
                n.MouseOverEvent -= N_MouseOverEvent;
            }
        }
    }
    private void N_MouseOverEvent(BasicSelectableItemData arg1, bool arg2)
    {
        _tgt.AssignItem(arg1, arg2);
    }

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



}


