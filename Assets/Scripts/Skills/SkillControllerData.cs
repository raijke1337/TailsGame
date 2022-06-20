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
    public SkillData GetSkillData { get; }

    private Timer _recTimer;
    private bool _isReady = true;
    public float GetCD => _recTimer.GetRemaining;



    

    // todo for ui

    public string SkillUpgradeID { get; set; } // todo implement 


    public virtual bool RequestUse()
    {
        bool result = _isReady;
        if (_isReady)
        {
            _recTimer.ResetTimer();
            _isReady = false;
        }
        return result;
    }
    public virtual float Ticks(float time)
    {
        return _recTimer.TimerTick(time);
    }

    public SkillControllerData (SkillControllerDataConfig cfg)
    {
        ID = cfg.ID;
        SkillType = cfg.SkillType;
        GetSkillData = new SkillData(cfg.Data);
        _recTimer = new Timer(GetSkillData.Recharge);
        _recTimer.TimeUp += _recTimer_TimeUp;
    }
    private void _recTimer_TimeUp(Timer arg)
    {
        _isReady = true;
    }
    // all logic moved to SkillsPlacerMan class, here we only have cooldown checking and data

}

