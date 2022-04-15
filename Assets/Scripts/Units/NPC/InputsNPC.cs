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
using UnityEngine.AI;
using Zenject;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class InputsNPC : ControlInputsBase
{

    public void SwitchState(bool setting) => fsm.SetAI(setting);

    [SerializeField] protected string enemyStatsID = default;
    [SerializeField] protected State InitialState;
    [SerializeField] protected State DummyState;

    [SerializeField] protected List<Transform> patrolPoints;

    protected EnemyStats _enemyStats;
    protected NavMeshAgent _navMeshAg;

     private StateMachine fsm;



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (fsm == null || fsm.CurrentState == null || fsm.CurrentVelocity == null) return;
        Gizmos.color = fsm.CurrentState.StateGizmoColor;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * _enemyStats.LookSpereCastRange);
        Gizmos.DrawWireSphere(transform.position + transform.forward * _enemyStats.LookSpereCastRange, _enemyStats.LookSpereCastRadius);

        Gizmos.color = Color.blue ;
        Gizmos.DrawLine(transform.position, transform.position + fsm.CurrentVelocity.normalized);
        Gizmos.DrawWireSphere(fsm.NMAgent.destination,fsm.NMAgent.stoppingDistance);
    }
#endif


    protected override void OnEnable()
    {
        base.OnEnable();
        if (patrolPoints == null) Debug.LogError($"Set at least 1 patrol point for {_statsCtrl.GetDisplayName}, {name} ");

        _enemyStats = new EnemyStats(Extensions.GetAssetsFromPath<EnemyStatsConfig>(Constants.Configs.c_EnemyStatConfigsPath).First
    (t => t.ID == enemyStatsID));

        _navMeshAg = GetComponent<NavMeshAgent>();
        fsm = new StateMachine(_navMeshAg, _enemyStats,InitialState,DummyState);
        Bind(true);
        fsm.SetPatrolPoints(patrolPoints);
    }
    protected virtual void LateUpdate()
    {
        velocityVector = fsm.CurrentVelocity;
    }
    protected virtual void OnDisable()
    {
        Bind(false);
    }

    protected virtual void Bind(bool isStart)
    {
        if (isStart)
        {
            _handler.RegisterUnitForStatUpdates(fsm);
            fsm.AgressiveActionRequest += HandleAttackRequest;
            fsm.PlayerSeenEvent += HandlePlayerDetection;
        }
        else
        {
            _handler.RegisterUnitForStatUpdates(fsm, false);
            fsm.AgressiveActionRequest -= HandleAttackRequest;
            fsm.PlayerSeenEvent -= HandlePlayerDetection;
        }
    }

    protected virtual void HandlePlayerDetection()
    {
        Debug.Log($"Placeholder: {_statsCtrl.GetDisplayName} detected player; {this}");
    }
    protected virtual void HandleAttackRequest()
        // todo more logic
    {
        CombatActionSuccessCallback(CombatActionType.Melee);
    }
    public void Aggro(BaseUnit unit, bool startCombat = true) => fsm.OnAggro(unit, startCombat);



}

