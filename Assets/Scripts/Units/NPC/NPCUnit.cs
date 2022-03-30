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

public class NPCUnit : BaseUnit
{
    public Allegiance Side;
    private InputsNPC _npcController;

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
            _npcController.NPCdiedDisableAIEvent?.Invoke(_npcController);
        }
    }


}

