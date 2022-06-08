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
public class NPCUnit : BaseUnit, InteractiveItem
{
    private InputsNPC _npcController;
    //public event SimpleEventsHandler<NPCUnit> OnUnitAggro;
    

    public EnemyType GetEnemyType => _npcController.GetEnemyType;
    public RoomController UnitRoom
    {
        get => _npcController.UnitRoom;
        set => _npcController.UnitRoom = value;        
    }

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
    protected override void OnDisable()
    {
        base.OnDisable();

    }

    #region behavior

    public void AiToggle(bool isProcessing)
    {
        _npcController.SwitchState(isProcessing);
    }

    protected override void HealthChangedEvent(float value, float prevValue)
    {
        base.HealthChangedEvent(value, prevValue);
        UnitRoom.StartCombatCheck(this);
    }

    public void EnterCombat(PlayerUnit player, bool isCombat = true) => _npcController.EnterCombat(player, isCombat);
    #endregion
}

