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
    protected EnemyStats _enemyStats;
    public EnemyStats GetStats => _enemyStats;


    public SimpleEventsHandler<NPCUnitControllerAI> NPCdiedDisableAIEvent;
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

    public void SetAI(bool setting)
    {
        aiActive = setting;     
    }

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
    [HideInInspector] public int NextPatrolPointIndex;
    public void SetUpWaypoints(List<Transform> waypoints) { PatrolPoints = waypoints; } // todo 
    public List<Transform> PatrolPoints;

    [HideInInspector] public float idleTime = 2f;

    #endregion

    #region chase state
    [HideInInspector] public Transform ChaseTarget;
    public Transform Eyes;

    #endregion

    #region attacking

    private float _timeSinceAttack;
    public bool IsAttackReady() { return (_timeSinceAttack > _enemyStats.TimeBetweenAttacks); }

    #endregion


    /// <summary>
    /// Pluggable AI end
    /// </summary>
    /// 

    private void OnDrawGizmos()
    {
        if (CurrentState == null || EditorApplication.isPlaying == false) return;

        Gizmos.color = CurrentState.StateGizmoColor;


        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + NavMeshAg.velocity);
    }

    protected override void Awake()
    {
        base.Awake();
              // todo unify code

        var cfg = Extensions.GetAssetsFromPath<EnemyStatsConfig>(Constants.Configs.c_EnemyStatConfigsPath).First
    (t => t.ID == enemyStatsID);

        _enemyStats = new EnemyStats(cfg);
        NavMeshAg = GetComponent<NavMeshAgent>();

        if (Eyes == null) Debug.LogError("Set eyes empty");
    }
    private void Update()
    {
        if (!aiActive) return;


        CurrentState.UpdateState(this);
        NavMeshAg.speed = Unit.GetStats()[StatType.MoveSpeed].GetCurrent();
        _movement = NavMeshAg.velocity;
        UpdateStateTimers();
    }

    protected override void UnitDiedAction(BaseUnit unit)
    {
        NPCdiedDisableAIEvent?.Invoke(this);
        _movement = Vector2.zero;
        NavMeshAg.isStopped = true;
    }

    public virtual void AttackRequest()
    {
        if (!IsAttackReady()) return;

        // todo more complex logic here
        if (_weaponCtrl.UseWeaponCheck(WeaponType.Melee))
        {
            _timeSinceAttack = 0f;
            CombatActionSuccessEvent?.Invoke(CombatActionType.Melee);
        }
    }



}

