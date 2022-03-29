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

public class StatsUpdatesHandler : MonoBehaviour
{
    private List<IStatsComponentForHandler> _list= new List<IStatsComponentForHandler>(); 
    public void RegisterUnitForStatUpdates(IStatsComponentForHandler stats, bool IsAdd = true)
    {
        if (!IsAdd)
        {
            _list.Remove(stats);
            return;
        }
        else        
        {
            _list.Add(stats);
            stats.SetupStatsComponent();
        }
    }

    private void Update()
    {
        foreach (var u in _list.ToList())
        {
            u.UpdateInDelta(Time.deltaTime);      
        }
    }



}

