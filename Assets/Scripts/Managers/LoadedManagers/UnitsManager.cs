using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : LoadedManagerBase
{
    private List<RoomController> _rooms = new List<RoomController>();

    private List<NPCUnit> _npcs = new List<NPCUnit>();
    private PlayerUnit _player;
    public PlayerUnit GetPlayerUnit { get => _player; }
    public List<NPCUnit> GetNPCs() => _npcs;
    public SkillRequestedEvent RequestToPlaceSkills;

    [SerializeField] private List<BaseUnit> _allUnits = new List<BaseUnit>();



    public override void Initiate()
    {
        _player = FindObjectOfType<PlayerUnit>();
        if (_player == null)
        {
            Debug.Log("No player found, aborting init");
            return;
        }
        _allUnits.Add(_player);
        InitializeUnit(_player);

        _rooms.AddRange(FindObjectsOfType<RoomController>());
        if (_rooms.Count == 0)
        {
            Debug.LogWarning($"No rooms found! Create some collider boxes with Room Controller");
        }

        foreach (var room in _rooms)
        {
            room.UnitFound += AddNPCToList;
            room.Initiate();
        }
    }

    private void AddNPCToList(NPCUnit n)
    {
        _npcs.Add(n);
        _allUnits.Add(n);
        InitializeUnit(n);
        SetAIStateUnit(true, n);

    }


    public override void RunUpdate(float delta)
    {
        foreach (var u in _allUnits)
        {
            u.RunUpdate(delta);
        }

        foreach (var r in _rooms)
        {
            r.RunUpdate(delta);
        }
    }

    public override void Stop()
    {
        foreach (var r in _rooms)
        {
            r.UnitFound -= AddNPCToList;
            r.Stop();
        }
        foreach (var u in _allUnits)
        {
            u.DisableUnit();
        }
    }


    private void InitializeUnit(BaseUnit u)
    {
        u.BaseUnitDiedEvent += (t) => HandleUnitDeath(t);
        u.SkillRequestSuccessEvent += (id, user, where) => RequestToPlaceSkills?.Invoke(id, user, where);
        u.InitiateUnit();
    }

    private void SetAIStateUnit(bool isProcessing, NPCUnit unit)
    {
        unit.AiToggle(isProcessing);
    }


    private void HandleUnitDeath(BaseUnit unit)
    {
        unit.DisableUnit();
        if (unit is NPCUnit)
        {
            SetAIStateUnit(false, unit as NPCUnit);

        }
        else
        {
            GameManager.Instance.OnPlayerDead();
        }
    }

}

