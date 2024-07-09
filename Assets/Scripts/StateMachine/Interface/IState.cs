// a predicate is a function that tests a condition and then returns a bool
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.StateMachine
{
    public interface IState
    {
        void Update(float d);
        void FixedUpdate(float d);
        void OnEnterState();
        void OnLeaveState();
        void HandleCombatAction(UnitActionType action);
        float TimeLeft { get; }
    }

}

