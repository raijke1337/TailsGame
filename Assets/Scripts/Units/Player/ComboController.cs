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
[Serializable]
public class ComboController : IStatsComponentForHandler
{
    protected StatValueContainer HeatContainer;
    protected float Degen;

    public ComboController (string ID)
    {
        var cfg = Extensions.GetAssetsFromPath<HeatStatsConfig>(Constants.Configs.c_ComboConfigsPath).First(t=>t.ID == ID);
        HeatContainer = new StatValueContainer(cfg.HeatContainer);
        Degen = cfg.DegenCoeff; 
    }

    public void SetupStatsComponent()
    {
        HeatContainer.Setup();
    }

    public void UpdateInDelta(float deltaTime)
    {
        throw new NotImplementedException();
    }
}

