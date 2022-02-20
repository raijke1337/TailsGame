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

public class UnitState : MonoBehaviour
{
    // not mono, used to handle stat changing commands and statuses
    [SerializeField]
    private UnitStatsHandler _statsHandler;
    // just a forwarder from handler
    private IReadOnlyDictionary<StatType, StatContainer> _statsDict;
    public IReadOnlyDictionary<StatType, StatContainer> GetAllCurrentStats => _statsDict;


    public string DisplayName { get; set; }

    public float CurrentHP { get; set; }
    public float CurrentShield { get; set; }
    public float CurrentHeat { get; set; }
    public float CurrentMoveSpeed { get; set; }

    public bool IsInDodge { get; set; }
    public bool IsShielded { get; set; }

    [SerializeField]
    public Allegiance Side { get; set; }

    // for interactions
    public ICommandsAssistant GetCommandsAssistant => _statsHandler;
    public IStatusAssistant GetStatusAssistant => _statsHandler; // this is todo, does nothing


    private void Start()
    {
        // set up initial stats from attached dictionary in Handler
        _statsDict = _statsHandler.GetStats;

        UpdateStatValues();

        _statsHandler.OwnerName = name;
        CurrentHP = _statsDict[StatType.Health].GetCurrentValue; 
        CurrentShield = _statsDict[StatType.Shield].GetCurrentValue / 2; //todo check if applicable
        CurrentHeat = _statsDict[StatType.Heat].GetCurrentValue;
        CurrentMoveSpeed = _statsDict[StatType.MoveSpeed].GetCurrentValue;


    }

    // this should be called by everything else to get relevant stats
    public StatContainer GetStatContainer(StatType type)
    {
#if UNITY_EDITOR
        if (!_statsDict.ContainsKey(type)) Debug.LogError($"{DisplayName} has no stat {type}, but it was requested");
#endif
        return _statsDict[type];
    }
       
    
    [ContextMenu("Update Values")]
    private void UpdateStatValues()
    {
        foreach (var st in _statsDict)
        {
            st.Value.UpdateStatValue();
        }
    }

    #region UnityEditor
#if UNITY_EDITOR
    [ContextMenu("Set up default stat values")]
    private void SetupDefaultStats()
    {
        var field = _statsHandler.GetType().GetField(
            "_dict", System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic);

        var dict = field.GetValue(_statsHandler) as StatsDictionary;

        if (!dict.ContainsKey(StatType.Health)) dict.Add(StatType.Health, new StatContainer());
        if (!dict.ContainsKey(StatType.HealthRegen)) dict.Add(StatType.HealthRegen, new StatContainer());
        if (!dict.ContainsKey(StatType.Shield)) dict.Add(StatType.Shield, new StatContainer());
        if (!dict.ContainsKey(StatType.ShieldRegen)) dict.Add(StatType.ShieldRegen, new StatContainer());
        if (!dict.ContainsKey(StatType.CritChance)) dict.Add(StatType.CritChance, new StatContainer());
        if (!dict.ContainsKey(StatType.CritMult)) dict.Add(StatType.CritMult, new StatContainer());
        if (!dict.ContainsKey(StatType.DashCount)) dict.Add(StatType.DashCount, new StatContainer());
        if (!dict.ContainsKey(StatType.DashRange)) dict.Add(StatType.DashRange, new StatContainer());
        if (!dict.ContainsKey(StatType.MoveSpeed)) dict.Add(StatType.MoveSpeed, new StatContainer());

        if (!dict.ContainsKey(StatType.Heat)) dict.Add(StatType.Heat, new StatContainer());
        if (!dict.ContainsKey(StatType.HeatRegen)) dict.Add(StatType.HeatRegen, new StatContainer());

    }
#endif
    #endregion



}

