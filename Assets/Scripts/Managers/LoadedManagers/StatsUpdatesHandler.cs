using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsUpdatesHandler : LoadedManagerBase
{


    private List<IStatsComponentForHandler> _list;

    public override void Initiate()
    {

    }
    public override void RunUpdate(float delta)
    {
        if (_list == null) return;
        foreach (var u in _list.ToList())
        {
            u.UpdateInDelta(Time.deltaTime);
        }
    }

    public override void Stop()
    {
        foreach (var u in _list)
        {
            u.StopStatsComponent();
        }
    }


    public void RegisterUnitForStatUpdates(IStatsComponentForHandler stats, bool IsAdd = true)
    {
        if (stats is StateMachine) Debug.LogError($"Registering {stats} in {this}, not supposed to happen!");

        if (_list == null) _list = new List<IStatsComponentForHandler>();
        if (!IsAdd)
        {
            Debug.Log($"Unregistered {stats}");
            _list.Remove(stats);
            return;
        }
        else
        {
            stats.SetupStatsComponent();
            if (!_list.Contains(stats))
            {
                _list.Add(stats);
                //Debug.Log($"Registered {stats}");
                // to prevent double registration for weapons ctrl
            }
        }
    }


}

