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
using System.Threading.Tasks;

[RequireComponent(typeof(NavMeshAgent))]
public class InputsNPC : ControlInputsBase
{
    public RoomController UnitRoom { get; set; }
    [SerializeField] protected List<Transform> patrolPoints;
    public void SwitchState(bool setting) => fsm.SetAI(setting);

    [SerializeField] protected State InitialState;
    [SerializeField] protected State DummyState;
    [SerializeField, ReadOnly] protected State CurrentState;

    protected EnemyStats _enemyStats;
    protected NavMeshAgent _navMeshAg;
    private StateMachine fsm;
    public EnemyType GetEnemyType => _enemyStats.EnemyType;


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
    public override void BindControllers(bool isEnable)
    {
        base.BindControllers(isEnable);

        if (patrolPoints.Count == 0)
        {
            patrolPoints.Add(transform);
        }

        _enemyStats = new EnemyStats(Extensions.GetAssetsFromPath<EnemyStatsConfig>(Constants.Configs.c_EnemyStatConfigsPath).First
    (t => t.ID == _statsCtrl.GetUnitID));

        _navMeshAg = GetComponent<NavMeshAgent>();
        fsm = new StateMachine(_navMeshAg, _enemyStats, InitialState, DummyState);

        _navMeshAg.speed = _statsCtrl.GetBaseStats[BaseStatType.MoveSpeed].GetCurrent();
        _navMeshAg.stoppingDistance = _enemyStats.AttackRange;

        Bind(isEnable);
        fsm.SetPatrolPoints(patrolPoints);

    }

    protected virtual void LateUpdate()
    {
        // todo too many bandaids
        if (fsm == null) return;
        CurrentState = fsm.CurrentState;
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
            fsm.AllyRequest += HandleAllySearch;
            fsm.RotateRequest += HandleRotation;
            _statsCtrl.GetBaseStats[BaseStatType.MoveSpeed].ValueChangedEvent += (current, old) => _navMeshAg.speed = current;
        }
        else
        {
            _handler.RegisterUnitForStatUpdates(fsm, false);
            fsm.AgressiveActionRequest -= HandleAttackRequest;
            fsm.AllyRequest -= HandleAllySearch;
            _statsCtrl.GetBaseStats[BaseStatType.MoveSpeed].ValueChangedEvent -= (current, old) => _navMeshAg.speed = current;
            fsm.RotateRequest -= HandleRotation;
        }
    }

    protected virtual void HandleAttackRequest(CombatActionType type)
    { 
        CombatActionSuccessCallback(type);
    }
    protected virtual void HandleRotation() => LerpRotateToTarget(fsm.FoundPlayer.transform.position);
    public void Aggro(PlayerUnit unit, bool startCombat = true) => fsm.OnAggro(unit, startCombat);
    protected virtual void HandleAllySearch()
    {
        var found = UnitRoom.GetBigRobot();
        fsm.SetAlly(found);
    }

}

