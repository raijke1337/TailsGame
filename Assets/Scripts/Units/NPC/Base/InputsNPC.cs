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

[RequireComponent(typeof(NavMeshAgent), typeof(Collider), typeof(Rigidbody))]
public abstract class InputsNPC : ControlInputsBase
{
#if UNITY_EDITOR
    [SerializeField, Tooltip("Show action data in console")] public bool DebugEnabled = false;
#endif


    public RoomController UnitRoom { get; set; }
    [SerializeField] protected List<Transform> patrolPoints;
    public void SwitchState(bool setting) => fsm.SetAI(setting);

    [SerializeField] protected State InitialState;
    [SerializeField] protected State DummyState;
    [SerializeField] protected State CurrentState;

    protected EnemyStats _enemyStats;
    protected NavMeshAgent _navMeshAg;
    [SerializeField] protected StateMachine fsm;

    public override UnitType GetUnitType() => _enemyStats.EnemyType;
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
        fsm = new StateMachine(_navMeshAg, _enemyStats, InitialState, DummyState,Unit);

        _navMeshAg.speed = _statsCtrl.GetBaseStats[BaseStatType.MoveSpeed].GetCurrent;
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

#if UNITY_EDITOR
        if (DebugEnabled)
        {
            if (UnitRoom == null) Debug.LogError($"{this} has no room assigned");
            fsm.Debugging = DebugEnabled;
        }
#endif

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
            fsm.AgressiveActionRequestSM += HandleAttackRequest;
            fsm.PlayerSpottedSM += OnPlayerSpottedSM;
            fsm.CombatPreparationSM += Fsm_CombatPreparationSM;
            fsm.AggroRequestedSM += Fsm_AggroRequestedSM;
            _statsCtrl.GetBaseStats[BaseStatType.MoveSpeed].ValueChangedEvent += (current, old) => _navMeshAg.speed = current;
            fsm.RotationRequestedSM += Fsm_RotationRequestedSM;
        }
        else
        {
            _handler.RegisterUnitForStatUpdates(fsm, false);
            fsm.AgressiveActionRequestSM -= HandleAttackRequest;
            _statsCtrl.GetBaseStats[BaseStatType.MoveSpeed].ValueChangedEvent -= (current, old) => _navMeshAg.speed = current;
            fsm.PlayerSpottedSM -= OnPlayerSpottedSM;
            fsm.CombatPreparationSM -= Fsm_CombatPreparationSM;
            fsm.AggroRequestedSM -= Fsm_AggroRequestedSM; 
            fsm.RotationRequestedSM -= Fsm_RotationRequestedSM;
        }
    }
    #region state machine

    private void Fsm_RotationRequestedSM()
    {
        RotateToSelectedUnit();
    }

    protected virtual void Fsm_AggroRequestedSM()
    {
        fsm.SelectedUnit = UnitRoom.GetUnitForAI(UnitType.Player);
    }

    protected virtual void OnPlayerSpottedSM(PlayerUnit arg)
    {
        (Unit as NPCUnit).OnUnitSpottedPlayer();
        fsm.SelectedUnit = arg;
    }


    //attack action logic is here
    protected virtual void HandleAttackRequest(CombatActionType type)
    {
        bool success;
        string text;

        switch (type)
        {
            case CombatActionType.Melee:
                success = _weaponCtrl.UseWeaponCheck(WeaponType.Melee, out text);
                if (success) CombatActionSuccessCallback(type);
                break;
            case CombatActionType.Ranged:
                success = _weaponCtrl.UseWeaponCheck(WeaponType.Ranged, out text);
                if (success) CombatActionSuccessCallback(type);
                break;
            case CombatActionType.Dodge:
                text = ($"{Unit.GetFullName} requested {type} but {this} has no dodge controller implemented");
                break;
            case CombatActionType.MeleeSpecialQ:
                if (_skillCtrl.RequestSkill(type, out _)) CombatActionSuccessCallback(type);
                break;
            case CombatActionType.RangedSpecialE:
                if (_skillCtrl.RequestSkill(type, out _)) CombatActionSuccessCallback(type);
                break;
            case CombatActionType.ShieldSpecialR:
                if (_skillCtrl.RequestSkill(type, out _)) CombatActionSuccessCallback(type);
                break;
        }
    }

    protected virtual void RotateToSelectedUnit()
    {      
        LerpRotateToTarget(fsm.SelectedUnit.transform.position);
    }
    protected abstract void Fsm_CombatPreparationSM();




    #endregion

    #region room manager

    public void ForceCombat(PlayerUnit player) => fsm.SelectedUnit = player;

    #endregion










#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!DebugEnabled || fsm == null || CurrentState == null) return;
        // state gizmos
        Gizmos.color = CurrentState.StateGizmoColor;
        Gizmos.DrawSphere(fsm.EyesEmpty.position, 0.1f);
        Gizmos.DrawLine(fsm.EyesEmpty.position,fsm.EyesEmpty.position+fsm.EyesEmpty.forward*_enemyStats.LookSpereCastRange);
        //navmesh gizmos
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(fsm.NMAgent.transform.position, fsm.NMAgent.transform.position+fsm.NMAgent.transform.forward);
    }
#endif







}

