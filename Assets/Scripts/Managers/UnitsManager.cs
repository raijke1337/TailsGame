using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    private List<RoomController> _rooms = new List<RoomController>();

    private List<NPCUnit> _units = new List<NPCUnit>();
    private PlayerUnit _player;
    public PlayerUnit GetPlayerUnit { get => _player; }
    public List<NPCUnit> GetNPCs() => _units;
    public SkillRequestedEvent RequestToPlaceSkills;

    private void Awake()
    {
        _units.AddRange(FindObjectsOfType<NPCUnit>());
        _player = FindObjectOfType<PlayerUnit>();

        _rooms.AddRange(FindObjectsOfType<RoomController>());
        foreach (var room in _rooms)
        {
            room.Manager = this;
        }

        foreach (var npc in _units)
        {
            npc.BaseUnitDiedEvent += (t) => HandleUnitDeath(t);
            npc.SkillRequestSuccessEvent += (id, user) => RequestToPlaceSkills?.Invoke(id, user);
            
        }
        _player.BaseUnitDiedEvent += HandleUnitDeath;
        _player.SkillRequestSuccessEvent += (id, user) => RequestToPlaceSkills?.Invoke(id, user);
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
        foreach (var npc in _units)
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


}

