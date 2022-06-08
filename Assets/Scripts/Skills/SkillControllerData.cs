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

    // todo for ui
    public float GetCD => _recTimer.GetRemaining;
    public Sprite GetSkillImage => _data.Icon;

    public string SkillUpgradeID { get; set; } // todo implement 


    public virtual bool RequestUse()
    {
        if (_isReady)
        {
            _recTimer.ResetTimer();
            _isReady = false;
        }
        return _isReady;
    }
    public virtual float Ticks(float time)
    {
        return _recTimer.TimerTick(time);
    }

    public SkillControllerData (SkillControllerDataConfig cfg)
    {
        ID = cfg.ID;
        SkillType = cfg.SkillType;
        _data = new SkillData(cfg.Data);
        _recTimer = new Timer(_data.Recharge);
        _recTimer.TimeUp += _recTimer_TimeUp;
    }
    private void _recTimer_TimeUp(Timer arg)
    {
        _isReady = true;
    }
    // all logic moved to SkillsPlacerMan class, here we only have cooldown checking and data

}

