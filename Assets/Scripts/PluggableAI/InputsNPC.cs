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
    [Inject] private UnitsManager _manager;

    [SerializeField] protected string enemyStatsID = default;
    protected EnemyStats _enemyStats;
    public EnemyStats GetStats => _enemyStats;
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
        NavMeshAg.isStopped = true;
        NavMeshAg.destination = transform.position;
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
        Gizmos.DrawLine(Eyes.transform.position, Eyes.transform.position + Eyes.transform.forward*_enemyStats.LookSpereCastRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + NavMeshAg.velocity);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
              // todo unify code

        _enemyStats = new EnemyStats(Extensions.GetAssetsFromPath<EnemyStatsConfig>(Constants.Configs.c_EnemyStatConfigsPath).First
    (t => t.ID == enemyStatsID));

        if (Eyes == null) Debug.LogError("Set eyes empty");
        NavMeshAg = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (!aiActive) return;
        CurrentState.UpdateState(this);
        velocityVector = NavMeshAg.velocity;
        UpdateStateTimers();
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

