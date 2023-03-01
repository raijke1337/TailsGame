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
        
    }

    public void RegisterUnitForStatUpdates(IStatsComponentForHandler stats, bool IsAdd = true)
    {
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
                Debug.Log($"Registered {stats}");
                // to prevent double registration for weapons ctrl
            }
        }
    }


}

