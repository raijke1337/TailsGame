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

public class NPCUnit : BaseUnit,InteractiveItem
{
    public Allegiance Side;
    private InputsNPC _npcController;


    public SimpleEventsHandler<NPCUnit> UnitDiedEvent;



    protected override void OnEnable()
    {
        base.OnEnable();
        if (!CompareTag("Enemy"))
            Debug.LogWarning($"Set enemy tag for{name}");
        _npcController = _controller as InputsNPC;
    }


    protected override void HealthChangedEvent(float value)
    {
        base.HealthChangedEvent(value);
        if (value <= 0f)
        {
            _npcController.SetAI(false);
        }
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
    public event MouseOverEvents SelectionEvent;
    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectionEvent?.Invoke(this, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SelectionEvent?.Invoke(this, false);
    }
}

