using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DodgeController : IStatsComponentForHandler
{

    Dictionary<DodgeStatType, StatValueContainer> _stats;

    public IReadOnlyDictionary<DodgeStatType,StatValueContainer> GetDodgeStats { get { return _stats; } }
    public int GetDodgeCharges() =>  _stats != null ? (int)_stats[DodgeStatType.Charges].GetCurrent : 0; 

    private Queue<Timer> _timerQueue = new Queue<Timer>();


    public DodgeController(string unitID)
    {
        _stats = new Dictionary<DodgeStatType, StatValueContainer>();
        var cfg = Extensions.GetAssetsFromPath<DodgeStatsConfig>(Constants.Configs.c_DodgeConfigsPath).First(t=>t.ID == unitID);
        foreach (var c in cfg.Stats)
        {
            _stats[c.Key] = new StatValueContainer(c.Value);
        }
    }
    public void SetupStatsComponent()
    {
        foreach (var st in _stats.Values)
        { st.Setup(); }        
    }


    public bool IsDodgePossibleCheck()
    {
        if (_stats[DodgeStatType.Charges].GetCurrent == 0f) return false;
        else
        {
            _stats[DodgeStatType.Charges].ChangeCurrent(-1);
            var t = new Timer(_stats[DodgeStatType.Cooldown].GetCurrent);
            _timerQueue.Enqueue(t);
            t.TimeUp += T_TimeUp;
            return true;
        }
    }

    private void T_TimeUp(Timer arg)
    {
        _timerQueue.Dequeue();
        _stats[DodgeStatType.Charges].ChangeCurrent(1);
    }

    public void UpdateInDelta(float deltaTime)
    {
        foreach (var timer in _timerQueue) timer.TimerTick(deltaTime);
    }


    //private List<Timer> _timers;    // no coroutines
    //public bool IsDodgePossibleCheck()
    //{
    //    if (_stats[DodgeStatType.Charges].GetCurrent() == 0f) return false;
    //    else
    //    {
    //        _stats[DodgeStatType.Charges].ChangeCurrent(-1);
    //        _timers.Add(new Timer(_stats[DodgeStatType.Cooldown].GetCurrent()));
    //        return true;
    //    }
    //}

    //public void UpdateInDelta(float deltaTime)
    //{
    //    foreach (var t in _timers)
    //    {
    //        if (t.time <= 0f)
    //        {
    //            _stats[DodgeStatType.Charges].ChangeCurrent(1);
    //            _timers.Remove(t);
    //            return;
    //        }
    //        else
    //        {
    //            t.time -= deltaTime;
    //        }
    //    }
    //}

}



