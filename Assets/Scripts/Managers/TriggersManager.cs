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
using Zenject;

public class TriggersManager : MonoBehaviour
{
    private List<BaseTrigger> _triggers;

    [SerializeField]
    private List<BaseStatTriggerConfig> _configs;

    [Inject]
    private PlayerUnit _player;

    private void Start()
    {
        _triggers = new List<BaseTrigger>();
        _triggers.AddRange(FindObjectsOfType<BaseTrigger>());
        UpdateDatas();
    }


    [ContextMenu(itemName:"Get trigger configs manually")]
    public void UpdateDatas()
    {
        _configs = Extensions.GetAssetsFromPath<BaseStatTriggerConfig>(Constants.c_TriggersConfigsPath);
    }


    // todo??
    public void Activation(string ID, BaseUnit target)
    {
        var config = _configs.First(t => t.ID == ID);
        switch (config.TargetType)
        {
            case TriggeredEffectTargetType.Target:
                target.ApplyEffect(new TriggeredEffect(config.ID, config.StatID, config.InitialValue, config.RepeatedValue,
config.RepeatApplicationDelay, config.TotalDuration, config.Icon));
                break;
            case TriggeredEffectTargetType.Self:
                _player.ApplyEffect(new TriggeredEffect(config.ID, config.StatID, config.InitialValue, config.RepeatedValue,
config.RepeatApplicationDelay, config.TotalDuration, config.Icon));
                break;
        }
;
    }



}

