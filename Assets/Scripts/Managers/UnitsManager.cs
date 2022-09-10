using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    private List<RoomController> _rooms = new List<RoomController>();

    private List<NPCUnit> _units = new List<NPCUnit>();
    private List<BaseUnit> _allUnits = new List<BaseUnit>();
    private PlayerUnit _player;
    public PlayerUnit GetPlayerUnit { get => _player; }
    public List<NPCUnit> GetNPCs() => _units;
    public SkillRequestedEvent RequestToPlaceSkills;

    [SerializeField] private ItemsEquipmentsHandler itemsHandler;


    private void Awake()
    {
        itemsHandler = new ItemsEquipmentsHandler(new List<IInventoryItem>(Extensions.GetAssetsOfType<ItemBase>(Constants.Combat.c_ItemPrefabsPath)));

        _units.AddRange(FindObjectsOfType<NPCUnit>());
        _player = FindObjectOfType<PlayerUnit>();

        _rooms.AddRange(FindObjectsOfType<RoomController>());
        foreach (var room in _rooms)
        {
            room.SetPlayer(_player);
        }
        _allUnits.Add(_player);
        _allUnits.AddRange(_units);       

        foreach (var u in _allUnits)
        {
            u.BaseUnitDiedEvent += (t) => HandleUnitDeath(t);
            u.SkillRequestSuccessEvent += (id, user, where) => RequestToPlaceSkills?.Invoke(id, user, where);
            u.InitInventory(itemsHandler);            
        }

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
}

