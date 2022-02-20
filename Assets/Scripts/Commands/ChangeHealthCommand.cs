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
using static BaseCommand;

public class ChangeHealthCommand : BaseCommand, IStartCommand, IUpdateCommand
{
    private float _delayCurrent;
    public float InitialValue { get; private set; }
    public float RepeatedValue { get; private set; }
    public float Delay { get; private set; }
    
    /// <summary>
    /// change health 
    /// </summary>
    /// <param name="initalV">initial change, dmg is negative</param>
    /// <param name="repeatedV">Dot or hot value</param>
    /// <param name="delay">delay between ticks</param>
    /// <param name="duration">total time</param>
    /// <param name="id">id</param>
    /// <param name="sprite">visual</param>
    public ChangeHealthCommand(float initalV, float repeatedV, float delay, EffectData data) : base(data)
    {
        InitialValue = initalV;
        RepeatedValue = repeatedV;
        Delay = delay;
        // to make sure periodic damage isn't done instantly on application
    }

    public void OnStartCommand()
    {
        Target.GetUnitState.CurrentHP += InitialValue;
        _delayCurrent = Delay;
    }

    public void OnUpdateCommand(float delta)
    {
        _delayCurrent -= delta;
        if (_delayCurrent <= 0f)
        {
            Target.GetUnitState.CurrentHP += RepeatedValue;
            _delayCurrent = Delay;
        }
    }


    public override BaseCommand Clone()
    {
        return new ChangeHealthCommand
            (InitialValue, RepeatedValue, Delay, new EffectData(Duration, ID, Sprite));
    }

}

