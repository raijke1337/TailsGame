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

public class BaseSkill
{

    private Timer _recTimer;

    private bool _isReady = true;

    public string ID;
    public CombatActionType SkillType;
    public float Recharge;
    public Sprite Icon;


    public virtual bool RequestUse()
    {
        if (_isReady)
        {
            DoSkillActions();
            _recTimer = new Timer(Recharge);
            _isReady = false;
            return true;
        }
        else
        {
            Debug.Log($"Skill {SkillType} not ready, wait {_recTimer.time}");
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

    public BaseSkill (BaseSkillSettings cfg)
    {
        ID = cfg.ID;
        SkillType = cfg.SkillType;
        Recharge = cfg.Recharge;
        Icon = cfg.Icon;
    }
    protected virtual void DoSkillActions()
    {
        Debug.Log($"Used {ID}");
    }
}

