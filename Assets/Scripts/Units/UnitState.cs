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

    [SerializeField]
    private UnitStatsHandler _statsHandler;

    private IReadOnlyDictionary<StatType, StatContainer> _statsDict;

    public string DisplayName { get; set; }

    public float CurrentHP { get; set; }
    public float CurrentShield { get; set; }
    public float CurrentHorny { get; set; }
    public float CurrentMoveSpeed { get; set; }

    public bool IsInDodge { get; set; }
    public bool IsShielded { get; set; }

    [SerializeField]
    public Allegiance Side { get; set; }




    // for interactions
    public ICommandsAssistant GetCommandsAssistant => _statsHandler;
    public IStatChangeAssistant GetStatChangeAssistant => _statsHandler; // this is todo, does nothing
    public IReadOnlyDictionary<StatType, StatContainer> GetAllCurrentStats => _statsDict;

    private void Start()
    {
        // set up initial stats from attached dictionary in Handler

        _statsDict = _statsHandler.GetStats;
        _statsHandler.OwnerName = name;

        foreach (var st in _statsDict)
        {
            st.Value.UpdateStatValue();
        }
        CurrentHP = _statsDict[StatType.Health].GetCurrentValue;
        CurrentShield = _statsDict[StatType.Shield].GetCurrentValue;
        CurrentHorny = _statsDict[StatType.Heat].GetCurrentValue;
        CurrentMoveSpeed = _statsDict[StatType.MoveSpeed].GetCurrentValue;
    }

    public StatContainer GetStatValueForCalculations(StatType type) => _statsDict[type];


    #region UnityEditor
#if UNITY_EDITOR
    [ContextMenu("Update Values")]
    private void UpdateStatValues()
    {
        foreach (var st in _statsDict)
        {
            st.Value.UpdateStatValue();
        }
    }


    [ContextMenu("Set up default stat values")]
    private void SetupDefaultStats()
    {
        var field = _statsHandler.GetType().GetField(
            "_dict", System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic);

        var dict = field.GetValue(_statsHandler) as StatsDictionary;

        if (!dict.ContainsKey(StatType.Health)) dict.Add(StatType.Health, new StatContainer(new StatRange(0,200)));
        if (!dict.ContainsKey(StatType.Shield)) dict.Add(StatType.Shield, new StatContainer(new StatRange(0, 100)));
        if (!dict.ContainsKey(StatType.MoveSpeed)) dict.Add(StatType.MoveSpeed, new StatContainer(new StatRange(0, 10)));
        if (!dict.ContainsKey(StatType.CritMult)) dict.Add(StatType.CritMult, new StatContainer(new StatRange(0, 100)));
        if (!dict.ContainsKey(StatType.DashRange)) dict.Add(StatType.DashRange, new StatContainer(new StatRange(0, 10)));
        if (!dict.ContainsKey(StatType.Heat)) dict.Add(StatType.Heat, new StatContainer(new StatRange(0, 100)));
        if (!dict.ContainsKey(StatType.ShieldRegen)) dict.Add(StatType.ShieldRegen, new StatContainer(new StatRange(0, 10)));
        if (!dict.ContainsKey(StatType.HealthRegen)) dict.Add(StatType.HealthRegen, new StatContainer(new StatRange(0, 10)));
        if (!dict.ContainsKey(StatType.HeatRegen)) dict.Add(StatType.HeatRegen, new StatContainer(new StatRange(0, 10)));
    }
#endif
    #endregion

}

