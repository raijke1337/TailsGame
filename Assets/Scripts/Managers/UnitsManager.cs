using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    private List<RoomController> _rooms = new List<RoomController>();

    private List<NPCUnit> _npcs = new List<NPCUnit>();
    private PlayerUnit _player;
    public PlayerUnit GetPlayerUnit { get => _player; }
    public List<NPCUnit> GetNPCs() => _npcs;
    public SkillRequestedEvent RequestToPlaceSkills;

    private ItemsEquipmentsHandler invHandler;

    private void Awake()
    {
        invHandler = GameManager.GetItemsHandler();
        _player = FindObjectOfType<PlayerUnit>();
        if (_player == null)
        {
            Debug.Log("No player found, aborting init");
            return;
        }
        UnitSubs(_player);

        _rooms.AddRange(FindObjectsOfType<RoomController>());
        if (_rooms.Count == 0)
        {
            Debug.Log($"No rooms found, {this} stopping init for npcs");
            return;
        }
        foreach (var room in _rooms)
        {
            room.SetPlayer(_player);
        }
        _npcs.AddRange(FindObjectsOfType<NPCUnit>());
        if (_npcs.Count == 0)
        {
            Debug.Log($"No npc found, {this} stopping init for npcs");
            return;
        }

        foreach (var npc in _npcs)
        {
            UnitSubs(npc);
        }

    }
    private void UnitSubs (BaseUnit u)
    {
        u.BaseUnitDiedEvent += (t) => HandleUnitDeath(t);
        u.SkillRequestSuccessEvent += (id, user, where) => RequestToPlaceSkills?.Invoke(id, user, where);
        u.InitInventory(invHandler);
    }


    private void Start()
    {
        SetAIStateGlobal(true);
    }
    private void OnDisable()
    {
        SetAIStateGlobal(false);
        // todo unsubs
    }
    public void SetAIStateGlobal(bool isProcessing)
    {
        foreach (var npc in _npcs)
        {
            SetAIStateUnit(isProcessing, npc);
        }
    }
    public void SetAIStateUnit(bool isProcessing, NPCUnit unit)
    {
        unit.AiToggle(isProcessing);
    }

    public void MenuOpened(bool isOpen = true)
    {
        if (isOpen) { Time.timeScale = 0f;  }
        else { Time.timeScale = 1f; }
    }
    // called in enable


    private void HandleUnitDeath(BaseUnit unit)
    {
        if (unit is NPCUnit)
        {
            SetAIStateUnit(false, unit as NPCUnit);
        }
        else
        {
            EditorApplication.isPaused = true;
            Debug.LogWarning("You died");
        }
    }
    public void OnStartGamePlay(bool isStart)
    {
        
    }


}

