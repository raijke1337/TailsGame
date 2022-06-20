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

[RequireComponent(typeof(InputsNPC)), RequireComponent(typeof(EnemyWeaponCtrl))]
public abstract class NPCUnit : BaseUnit, IInteractiveItem
{
    private InputsNPC _npcController;

    public void SetUnitRoom(RoomController room) => _npcController.UnitRoom = room;


    public event SimpleEventsHandler<NPCUnit> OnUnitAttackedEvent;
    public event SimpleEventsHandler<NPCUnit> OnUnitSpottedPlayerEvent;


    public InteractiveItemType IIType => InteractiveItemType.Enemy;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!CompareTag("Enemy"))
            Debug.LogWarning($"Set enemy tag for{name}");
        _npcController = _controller as InputsNPC;        
    }

    public override void ApplyEffect(TriggeredEffect eff)
    {
        switch (eff.StatID)
        {
            case BaseStatType.Health:
                _baseStats.AddTriggeredEffect(eff);
                break;
            case BaseStatType.MoveSpeed:
                _baseStats.AddTriggeredEffect(eff);
                break;
        }
    }

    public void AiToggle(bool isProcessing)
    {
        _npcController.SwitchState(isProcessing);
    }
    public virtual void ReactToDamage(PlayerUnit player)
    {
        _npcController.ForceCombat(player);
    }

    protected override void HealthChangedEvent(float value, float prevValue)
    {
        base.HealthChangedEvent(value, prevValue);
        OnUnitAttackedEvent?.Invoke(this);
    }
    public void OnUnitSpottedPlayer() => OnUnitSpottedPlayerEvent?.Invoke(this); 
}

