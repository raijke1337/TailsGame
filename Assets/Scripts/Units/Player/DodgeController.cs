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
public class DodgeController : IStatsComponentForHandler
{

    [SerializeField] SerializableDictionaryBase<DodgeStatType, StatValueContainer> _stats;
    public IReadOnlyDictionary<DodgeStatType,StatValueContainer> GetDodgeStats() => _stats;
    public int GetDodgeCharges() => (int)_stats[DodgeStatType.Charges].GetCurrent();



    public void SetupStatsComponent()
    {
        foreach (var st in _stats.Values)
        { st.Setup(); }
    }

    public bool IsDodgePossibleCheck()
    {
        if (_stats[DodgeStatType.Charges].GetCurrent() == 0f) return false;
        else
        {
            _stats[DodgeStatType.Charges].ChangeCurrent(-1);
            _timers.Add(new Timer(_stats[DodgeStatType.Cooldown].GetCurrent()));
            return true;
        }
    }

    private List<Timer> _timers = new List<Timer>();    // no coroutines

    public void UpdateInDelta(float deltaTime)
    {
        foreach (var t in _timers)
        {
            if (t.time <= 0f)
            {
                _stats[DodgeStatType.Charges].ChangeCurrent(1);
                _timers.Remove(t);
                return;
            }
            else
            {
                t.time -= deltaTime;
            }
        }
    }
}



