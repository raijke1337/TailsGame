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
public class SkillControllerData
{
    public string ID;
    public CombatActionType SkillType;

    private Timer _recTimer;
    private bool _isReady = true;

    private SkillData _data;
    public float GetCost => _data.SkillCost;

    public string SkillUpgradeID { get; set; } // todo implement 


    public virtual bool RequestUse()
    {
        if (_isReady)
        {
            _recTimer = new Timer(_data.Recharge);
            _isReady = false;
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual float Ticks(float time)
    {
        if (_recTimer == null) return 0f; 

        if (_recTimer.time <= 0f)
        {
            _isReady = true;
        }
        else { _recTimer.time -= time; }
        return _recTimer.time;
    }

    public SkillControllerData (SkillControllerDataConfig cfg)
    {
        ID = cfg.ID;
        SkillType = cfg.SkillType;
        _data = new SkillData(cfg.Data);
    }
    // all logic moved to SkillsPlacerMan class, here we only have cooldown checking and data

}

