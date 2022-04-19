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

[RequireComponent(typeof(InputsNPC)),RequireComponent(typeof(EnemyWeaponCtrl))]
public class NPCUnit : BaseUnit,InteractiveItem
{

    private InputsNPC _npcController;
    public RoomController UnitRoom { get; set; }

    public SimpleEventsHandler<NPCUnit> UnitDiedEvent;
    public SimpleEventsHandler<NPCUnit> UnitWasAttackedEventForAggro;
    public event MouseOverEvents SelectionEvent;


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
            case StatType.Health:
                _baseStats.AddTriggeredEffect(eff);
                break;
            case StatType.HealthRegen:
                _baseStats.AddTriggeredEffect(eff);
                break;
            case StatType.Heat:
                break;
            case StatType.HeatRegen:
                break;
            case StatType.MoveSpeed:
                _baseStats.AddTriggeredEffect(eff);
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectionEvent?.Invoke(this, true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SelectionEvent?.Invoke(this, false);
    }


    #region behavior

    public void AiToggle(bool isProcessing) => _npcController.SwitchState(isProcessing);


    protected override void HealthChangedEvent(float value, float prevValue)
    {
        base.HealthChangedEvent(value, prevValue);
        UnitWasAttackedEventForAggro?.Invoke(this);
    }

    public void SetChaseTarget(BaseUnit unit)
    {
        //Debug.Log($"{_baseStats.GetDisplayName} aggro on: {unit.GetFullName()}");
        _npcController.Aggro(unit);
    }



    #endregion

}

