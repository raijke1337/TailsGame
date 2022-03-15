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
public class DodgeController : IStatsComponentForHandler
{

    private PlayerUnitController _ctrl;

    [SerializeField] int Charges = 3;
    [SerializeField] float Recharge = 4f;

    //[SerializeField] float Duration = 1f;
    //[SerializeField] float Range = 3f;

    private List<DodgeCharge> _charges = new List<DodgeCharge>();

    public SimpleEventsHandler<bool> DodgeCalcCompleteEvent;


    public void Initialize(PlayerUnitController c)
    {
        _ctrl = c;
    }
    public void Setup()
    {
        for (int i = 0; i < Charges; i++)
        {
            _charges.Add(new DodgeCharge(Recharge));
        }
        _charges.TrimExcess();
    }

    public bool IsDodgePossibleCheck()
    {
        DodgeCharge charge = _charges.FirstOrDefault(t => t.CurrentCooldown <= 0f);
        if (charge == null)
        {
            return false;
        }
        else
        {
            charge.CurrentCooldown = Recharge;
            return true;
        }
    }

    public void UpdateInDelta(float deltaTime)
    {
        foreach (var ch in _charges)
        {
            if (ch.CurrentCooldown > 0f) { ch.CurrentCooldown -= deltaTime; }            
        }
    }


    public class DodgeCharge
    {
        public float CurrentCooldown = 0f;
        public float Recharge;
        public DodgeCharge(float cd) { Recharge = cd; }
    }
}



