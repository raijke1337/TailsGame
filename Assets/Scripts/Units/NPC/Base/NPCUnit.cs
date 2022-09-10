using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using RotaryHeart.Lib.SerializableDictionary;

[RequireComponent(typeof(InputsNPC))]
public abstract class NPCUnit : BaseUnit, IInteractiveItem
{
    private InputsNPC _npcController;


    [SerializeField] protected ItemsEquipmentsHandler.DroppableItem[] Drops;


    public void SetUnitRoom(RoomController room) => _npcController.UnitRoom = room;


    public event SimpleEventsHandler<NPCUnit> OnUnitAttackedEvent;
    public event SimpleEventsHandler<NPCUnit> OnUnitSpottedPlayerEvent;


    public InteractiveItemType IIType => InteractiveItemType.Enemy;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!CompareTag("Enemy"))
            Debug.LogWarning($"Set enemy tag for{name}");

        _controller.GetStatsController.GetBaseStats[BaseStatType.Health].ValueChangedEvent += OnOuch;

        if (Equipments.Count() == 0 || Equipments == null)
            Debug.LogWarning($"{this} has no equipments");
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

