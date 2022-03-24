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

public class BaseSkill : MonoBehaviour
{
    private float _recharge;
    private Timer _recTimer;

    private bool _isReady = true;

    public string ID;
    [SerializeField] private CombatActionType _type;

    public bool RequestUse()
    {
        if (_isReady)
        {
            Debug.Log($"Skill {_type} used");
            _recTimer = new Timer(_recharge);
            _isReady = false;
            return true;
        }
        else
        {
            Debug.Log($"Skill {_type} not ready, wait {_recTimer.time}");
            return false;
        }
    }
    public float Ticks(float time)
    {
        if (_recTimer == null) return 0f; 

        if (_recTimer.time <= 0f)
        {
            _isReady = true;
        }
        else { _recTimer.time -= time; }
        return _recTimer.time;
    }
}

