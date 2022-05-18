using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[Serializable]
public class StateMachine : IStatsComponentForHandler
{
    #region handler
    public void UpdateInDelta(float deltaTime)
    {
        if (!aiActive) return;
        CurrentState.UpdateState(this);
        CurrentVelocity = NMAgent.velocity;
        TimeInState += deltaTime;
    }
    public void SetupStatsComponent()
    {

    }
    #endregion

    [SerializeField] private bool aiActive = true;
    public State CurrentState { get; private set; }
    public State RemainState { get; private set; }
    public Vector3 CurrentVelocity { get; protected set; }
    public float TimeInState { get; private set; }
    public EnemyStats GetEnemyStats { get; private set; }
    public PlayerUnit FoundPlayer { get; private set; }
    public NPCUnit FoundAlly{ get; private set; }
    public bool InCombat { get; private set; }

    //public event StateMachineEvent PlayerSeenEvent;
    public event StateMachineEvent<CombatActionType> AgressiveActionRequest;
    public event StateMachineEvent AllyRequest;
    public event StateMachineEvent RotateRequest;



    public NavMeshAgent NMAgent { get; private set; }
    public Transform [] PatrolPoints;
    public int CurrentPatrolPointIndex = 0;


    public StateMachine(NavMeshAgent agent, EnemyStats stats, State init, State dummy)
    {
        NMAgent = agent;
        GetEnemyStats = stats;
        CurrentState = init;
        RemainState = dummy;
    }
    public void SetAI(bool setting)
    {
        aiActive = setting;
        if (NMAgent == null) return;
        NMAgent.isStopped = !setting;
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != RemainState)
        {
           CurrentState = nextState;
           OnExitState();
        }
    }
    private void OnExitState() => TimeInState = 0f;

    public void OnAggro(PlayerUnit unit,bool isStartCombat)
    {
        FoundPlayer = unit; InCombat = isStartCombat;
        // use this for PlayerSeenEvent functions
    }

    public void SetPatrolPoints(List<Transform> points)
    {
        PatrolPoints = new Transform[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            PatrolPoints[i] = points[i];
            //Debug.Log($"Set patrol point index {i}, {points[i]}");
        }
    }
    public void OnPatrolPointReached()
    {
        CurrentPatrolPointIndex++;
        if (CurrentPatrolPointIndex == PatrolPoints.Length) CurrentPatrolPointIndex = 0;
    }
    public bool CanSeePlayerCast()
    {
        Physics.SphereCast(NMAgent.transform.position, GetEnemyStats.LookSpereCastRadius, NMAgent.transform.forward, out var hit, GetEnemyStats.LookSpereCastRange);
        if (hit.collider == null) return false;
        var result = hit.collider.CompareTag("Player");
        if (result)
        {
            OnAggro(hit.collider.gameObject.GetComponent<PlayerUnit>(), true);
        }
        return result;
    }
    public void OnAttackRequest(CombatActionType type = CombatActionType.Melee) => AgressiveActionRequest?.Invoke(type);
    public void OnRequestAlly() => AllyRequest?.Invoke();
    public void SetAlly(NPCUnit ally) => FoundAlly = ally;
    public void OnRotate() => RotateRequest?.Invoke();

}
