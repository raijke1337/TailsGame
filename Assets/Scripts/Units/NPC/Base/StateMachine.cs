using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Arcatech.Units.Inputs
{
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
            if (wasSelectedUnitUpdated) OnUpdatedUnit();
        }
        public void SetupStatsComponent()
        {
            var eyes = new GameObject("SphereCaster");
            eyes.transform.SetPositionAndRotation(StateMachineUnit.transform.position, StateMachineUnit.transform.rotation);
            eyes.transform.position += (Vector3.up * GetEnemyStats.LookSpereCastRadius); // move up to not cast at floor only
            eyes.transform.SetParent(StateMachineUnit.transform, true);

            EyesEmpty = eyes.transform;
        }
        public bool IsReady { get => true; }

        public void StopStatsComponent()
        {

        }

        #endregion


        [SerializeField] private bool aiActive = true;

        public State CurrentState { get; private set; }
        public State RemainState { get; private set; }
        public Vector3 CurrentVelocity { get; protected set; }
        public float TimeInState { get; private set; }
        public EnemyStats GetEnemyStats { get; private set; }
        public BaseUnit StateMachineUnit { get; }
        // set by inputs , bool for potential checks
        private bool wasSelectedUnitUpdated;
        private BaseUnit _selectedUnit;
        public BaseUnit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                wasSelectedUnitUpdated = false;
            }
        }
        public BaseUnit FocusUnit { get; set; }


        private void OnUpdatedUnit() { wasSelectedUnitUpdated = true; }


        public event StateMachineEvent<CombatActionType> AgressiveActionRequestSM;
        public event StateMachineEvent<CombatActionType> ChangeRangeActionRequestSM;
        public event StateMachineEvent<PlayerUnit> PlayerSpottedSM;
        public event StateMachineEvent<UnitType> RequestFocusSM;
        public event StateMachineEvent AggroRequestedSM;
        public event StateMachineEvent RotationRequestedSM;

        public event SimpleEventsHandler<bool, IStatsComponentForHandler> ComponentChangedStateToEvent; // unused

        public NavMeshAgent NMAgent { get; }
        [HideInInspector] public Transform[] PatrolPoints;
        [HideInInspector] public int CurrentPatrolPointIndex = 0;

        public Transform EyesEmpty { get; private set; } // used for sphere casting to look

        #region setups
        public StateMachine(NavMeshAgent agent, EnemyStats stats, State init, State dummy, BaseUnit unit)
        {
            NMAgent = agent;
            GetEnemyStats = stats;
            CurrentState = init;
            RemainState = dummy;
            StateMachineUnit = unit;
        }
        public void SetAI(bool setting)
        {
            aiActive = setting;
            ComponentChangedStateToEvent?.Invoke(setting, this);

            if (NMAgent == null) return;
            NMAgent.isStopped = !setting;
        }

        public void TransitionToState(State nextState)
        {
            if (nextState != RemainState)
            {
#if UNITY_EDITOR
                if (Debugging) Debug.Log($"{StateMachineUnit} switched states: {CurrentState} -> {nextState}, time elapsed: {TimeInState}");
#endif
                CurrentState = nextState;
                OnExitState();
            }
        }
        private void OnExitState() => TimeInState = 0f;

        #endregion

        public void SetPatrolPoints(List<Transform> points)
        {
            PatrolPoints = new Transform[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                PatrolPoints[i] = points[i];
            }
        }
        public void OnPatrolPointReached()
        {
            CurrentPatrolPointIndex++;
            if (CurrentPatrolPointIndex == PatrolPoints.Length) CurrentPatrolPointIndex = 0;
        }
        public bool OnLookSphereCast()
        {
            Physics.SphereCast(EyesEmpty.position, GetEnemyStats.LookSpereCastRadius, EyesEmpty.forward, out var hit, GetEnemyStats.LookSpereCastRange);
            if (hit.collider == null) return false;
            var _coll = hit.collider;
#if UNITY_EDITOR
            if (Debugging)
            {
                string newtxt = $"Hit {_coll}";
                if (newtxt != sphereDebugTxt)
                {
                    sphereDebugTxt = newtxt;
                    //Debug.Log(sphereDebugTxt);
                }
            }
#endif
            var result = _coll.CompareTag("Player");
            if (result)
            {
                PlayerSpottedSM?.Invoke(_coll.GetComponent<PlayerUnit>());
            }
            return result;
        }
        public void OnAttackRequest(CombatActionType type)
        {
            AgressiveActionRequestSM?.Invoke(type);
        }
        public void OnSetFocus(UnitType type) => RequestFocusSM?.Invoke(type);
        public bool CheckIsInStoppingRange()
        {
            var result = Vector3.Distance(NMAgent.transform.position, NMAgent.destination) < NMAgent.stoppingDistance;
            return result;
        }
        public void OnAggroRequest() => AggroRequestedSM?.Invoke();
        public bool OnAllyNeedsHelp()
        {
            if (FocusUnit == null) return false;
            return FocusUnit.GetStats[BaseStatType.Health].GetCurrent / FocusUnit.GetStats[BaseStatType.Health].GetMax <= 0.5f;  // HUGE todo
        }
        public void OnRotateRequest() => RotationRequestedSM?.Invoke();
        public void OnSwapRanges(CombatActionType type) => ChangeRangeActionRequestSM?.Invoke(type);

        public void Ping()
        { }




#if UNITY_EDITOR
        [HideInInspector] public bool Debugging;
        string sphereDebugTxt;
#endif


    }
}