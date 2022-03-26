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

public class NPCUnitControllerAI : BaseUnitController
{
    [Inject] private UnitsManager _manager;

    [SerializeField] protected string enemyStatsID = default;
    [SerializeField] protected SerializableDictionaryBase<EnemyStatType, float> _enemyStats = new SerializableDictionaryBase<EnemyStatType, float>();
    public IReadOnlyDictionary<EnemyStatType, float> GetStats => _enemyStats;
    public SimpleEventsHandler<NPCUnitControllerAI> NPCdiedEvent;
    public override event SimpleEventsHandler<CombatActionType> CombatActionSuccessEvent;

    [HideInInspector] public NavMeshAgent NavMeshAg;

    /// <summary>
    /// Pluggable AI code here
    /// </summary> 
    [SerializeField] private State CurrentState;
    [SerializeField] private State RemainState;
    private float _timeInState;
    public bool IsStateCountdownElapsed(float time) { return _timeInState >= time; }

    // this is a dummy state the ai goes to if the DECISION is to stay in current state

    public void SetAI(bool setting) => aiActive = setting;
    private bool aiActive = true;

    public void TransitionToState(State nextState)
    {
        if (nextState != RemainState)
        {
            CurrentState = nextState;
            OnExitState();
        }
    }
    private void OnExitState() { _timeInState = 0; }
    private void UpdateStateTimers()
    {
        _timeInState += Time.deltaTime;
        _timeSinceAttack += Time.deltaTime;
    }

    #region patrol state
    [HideInInspector] public int NextPatrolPoint;
    public void SetUpWaypoints(List<Transform> waypoints) { PatrolPoints = waypoints; } // todo 
    public List<Transform> PatrolPoints;
    #endregion

    #region chase state
    [HideInInspector] public Transform ChaseTarget;
    public Transform Eyes;

    #endregion

    #region attacking

    private float _timeSinceAttack;
    public bool IsAttackReady() { return (_timeSinceAttack > _enemyStats[EnemyStatType.TimeBetweenAttacks]); }

    #endregion

    #region fleeing

    #endregion


    /// <summary>
    /// Pluggable AI end
    /// </summary>
    /// 

    private void OnDrawGizmos()
    {
        if (CurrentState == null || EditorApplication.isPlaying == false) return;
        if (_enemyStats == null) return;

        Gizmos.color = CurrentState.StateGizmoColor;
        Gizmos.DrawWireSphere(Eyes.position + Eyes.forward * _enemyStats[EnemyStatType.LookRange], _enemyStats[EnemyStatType.LookSpereCastRadius]);
        Gizmos.DrawLine(Eyes.position, (Eyes.position + Eyes.forward * _enemyStats[EnemyStatType.LookRange]));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + NavMeshAg.velocity);
    }

    protected override void Awake()
    {
        base.Awake();
              // todo unify code

        var cfg = Extensions.GetAssetsFromPath<EnemyStatsConfig>(Constants.c_EnemyStatConfigsPath).First
    (t => t.ID == enemyStatsID);


        _enemyStats.Add(EnemyStatType.LookRange, cfg.Stats[EnemyStatType.LookRange]);
        _enemyStats.Add(EnemyStatType.TimeBetweenAttacks, cfg.Stats[EnemyStatType.TimeBetweenAttacks]);
        _enemyStats.Add(EnemyStatType.LookSpereCastRadius, cfg.Stats[EnemyStatType.LookSpereCastRadius]);
        _enemyStats.Add(EnemyStatType.AttackRange, cfg.Stats[EnemyStatType.AttackRange]);
        _enemyStats.Add(EnemyStatType.ScanAngle, cfg.Stats[EnemyStatType.ScanAngle]);
        _enemyStats.Add(EnemyStatType.FleeHealth, cfg.Stats[EnemyStatType.FleeHealth]);



        NavMeshAg = GetComponent<NavMeshAgent>();
        if (Eyes == null) Debug.LogError("Set eyes empty");
    }
    private void Update()
    {
        if (!aiActive) return;
        CurrentState.UpdateState(this);
        _movement = NavMeshAg.velocity;
        UpdateStateTimers();
    }

    protected override void UnitDiedAction(BaseUnit unit)
    {
        NPCdiedEvent?.Invoke(this);
    }

    public virtual void AttackRequest()
    {
        // todo more complex logic here
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Melee))
        {
            _timeSinceAttack = 0f;
            CombatActionSuccessEvent?.Invoke(CombatActionType.Melee);
        }
    }



}

