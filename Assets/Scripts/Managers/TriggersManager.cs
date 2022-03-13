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
    [SerializeField]
    private Transform _triggersPool;
    [SerializeField]
    private LinkedList<BaseTriggerComp> _triggers = new LinkedList<BaseTriggerComp>();
    private void Start()
    {
        var _trig = _triggersPool.GetComponentsInChildren<BaseTriggerComp>();
        foreach (BaseTriggerComp trigger in _trig)
        {
            _triggers.AddLast(trigger);
        }
#if UNITY_EDITOR
        if (FindObjectsOfType<BaseTriggerComp>().Length != _triggers.Count)
            Debug.LogError("Item triggers must be in trigger pool");
#endif
    }

    // triggers use this to get data according to entered id in editor
    public T GetCommandByID<T>(string id) where T : BaseCommandEffect
    {
        return _config.CreateEffect(id) as T;

        // todo shitty implementation fix it
    }


    [Inject]
    private ChangeHealthCommandConf _config;


}

