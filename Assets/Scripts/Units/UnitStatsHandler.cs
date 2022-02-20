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
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class UnitStatsHandler : ICommandsAssistant , IStatusAssistant
{
    [SerializeField]
    public string OwnerName;
    [SerializeField]
    private StatsDictionary _dict;
    public IReadOnlyDictionary<StatType,StatContainer> GetStats => _dict;




#region commands

    LinkedList<BaseCommand> _currentCommands = new LinkedList<BaseCommand>();
    public IReadOnlyCollection<BaseCommand> GetAllCurrentlyActiveCommands => _currentCommands;
    public event SimpleEventsHandler<BaseCommand> OnCommandAppliedHandler;

    public void AddCommand(BaseCommand effect)
    {
        _currentCommands.AddLast(effect);
        OnCommandAppliedHandler?.Invoke(effect);
    }
    public void AddCommands(IEnumerable<BaseCommand> effects)
    {
        foreach (var ef in effects)
        {
            AddCommand(ef);
        }
    }
}
#endregion

// NYI for permanent changes
// ex move speed boost max health boost etc
public interface IStatusAssistant
{

}

// for effects that change stats - take dmg, heal, trap etc
public interface ICommandsAssistant
{
    void AddCommand(BaseCommand effect);
    void AddCommands(IEnumerable<BaseCommand> effects);
    IReadOnlyCollection<BaseCommand> GetAllCurrentlyActiveCommands { get; }
    event SimpleEventsHandler<BaseCommand> OnCommandAppliedHandler;
}
