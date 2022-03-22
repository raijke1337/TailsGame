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
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class SkillsController : IStatsComponentForHandler
{
    [SerializeField] private SerializableDictionaryBase<CombatActionType, BaseSkill> _skills;


    public bool RequestSkill (CombatActionType type)
    {
        return true;
        //return _skills[type].RequestUse();
    }

    public void Setup()
    {
        //throw new NotImplementedException();
    }

    public void UpdateInDelta(float deltaTime)
    {
        foreach (var sk in _skills.Values)
        {
            //sk.Ticks(deltaTime);
        }
    }
}

