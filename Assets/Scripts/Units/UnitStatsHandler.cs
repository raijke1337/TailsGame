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
public class UnitStatsHandler : ICommandsAssistant , IStatChangeAssistant
{

    [SerializeField]
    private StatsDictionary _dict;

    public IReadOnlyDictionary<StatType,StatContainer> GetStats => _dict;

    [SerializeField]
    public string OwnerName { get; set; }


    #region commands
    LinkedList<BaseCommand> _currentCommands = new LinkedList<BaseCommand>();
    public IReadOnlyCollection<BaseCommand> GetAllCurrentlyActiveCommands => _currentCommands;
    public event SimpleEventsHandler<BaseCommand> OnEffectEventHandler;

    public void AddStatChangeCommand(BaseCommand effect)
    {
        _currentCommands.AddLast(effect);
        OnEffectEventHandler?.Invoke(effect);
    }

    public void AddStatChangeCommands(IEnumerable<BaseCommand> effects)
    {
        foreach (var ef in effects)
        {
            _currentCommands.AddLast(ef);
            OnEffectEventHandler?.Invoke(ef);
        }
    }
}
#endregion

// NYI for permanent changes
// ex move speed boost max health boost etc
public interface IStatChangeAssistant
{

}

// for effects that change stats - take dmg, heal, trap etc
public interface ICommandsAssistant
{
    void AddStatChangeCommand(BaseCommand effect);
    void AddStatChangeCommands(IEnumerable<BaseCommand> effects);
    IReadOnlyCollection<BaseCommand> GetAllCurrentlyActiveCommands { get; }
    event SimpleEventsHandler<BaseCommand> OnEffectEventHandler;
}
