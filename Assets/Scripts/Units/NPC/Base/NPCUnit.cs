using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InputsNPC))]
public abstract class NPCUnit : BaseUnit
{
    private InputsNPC _npcController;
    [SerializeField] protected EquipmentBase[] NPCEquipments;


    public StateMachine GetStateMachine { get => _npcController.GetFSM; }
    public void SetUnitRoom(RoomController room) => _npcController.UnitRoom = room;


    public event SimpleEventsHandler<NPCUnit> OnUnitAttackedEvent;
    public event SimpleEventsHandler<NPCUnit> OnUnitSpottedPlayerEvent;

    public override void InitiateUnit()
    {
        base.InitiateUnit();
        if (!CompareTag("Enemy"))
            Debug.LogWarning($"Set enemy tag for{name}");

        _controller.GetStatsController.GetBaseStats[BaseStatType.Health].ValueChangedEvent += OnOuch;

        if (NPCEquipments.Count() == 0 || NPCEquipments == null)
            Debug.LogWarning($"{this} has no equipments");
        foreach (var item in NPCEquipments)
        {
            HandleStartingEquipment(item);
        }
    }

    private void OnOuch(float curr, float prev)
    {
        if (curr < prev) OnUnitAttackedEvent?.Invoke(this);
    }
    public void AiToggle(bool isProcessing)
    {
        if (_npcController == null) SetNPCInputs();
        _npcController.SwitchState(isProcessing);
        // todo thi is a bandaid
    }
    public virtual void ReactToDamage(PlayerUnit player)
    {
        _npcController.ForceCombat(player);
    }

    private void SetNPCInputs() => _npcController = _controller as InputsNPC;
    public void OnUnitSpottedPlayer() => OnUnitSpottedPlayerEvent?.Invoke(this);


}

