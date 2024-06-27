using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Arcatech.AI
{
    [Serializable]
    public class StateMachine : IManagedComponent
    {
        #region handler
        public void UpdateInDelta(float deltaTime)
        {
            if (ControlledUnit.LockUnit) return;
            CurrentState.UpdateState(this);
            CurrentVelocity = NMAgent.velocity;
            TimeInState += deltaTime;
            if (wasSelectedUnitUpdated) OnUpdatedUnit();
        }
        public void StartComp()
        {
            var eyes = new GameObject("SphereCaster");
            eyes.transform.SetPositionAndRotation(ControlledUnit.transform.position, ControlledUnit.transform.rotation);
            eyes.transform.position += (Vector3.up * GetEnemyStats.LookSphereRadius); // move up to not cast at floor only
            eyes.transform.SetParent(ControlledUnit.transform, true);

            EyesEmpty = eyes.transform;
        }
        public bool IsReady { get => true; }

        public void StopComp()
        {

        }

        #endregion


        // [SerializeField] private bool aiActive = true;
        [SerializeField] private bool _debugStates;
        public State CurrentState { get; private set; }
        public State RemainState { get; private set; }
        public Vector3 CurrentVelocity { get; protected set; }
        public float TimeInState { get; private set; }
        public EnemyStatsConfig GetEnemyStats { get; private set; }
        public ControlledUnit ControlledUnit { get; }
        // set by inputs , bool for potential checks
        private bool wasSelectedUnitUpdated;
        private BaseUnit _selectedUnit;
        public BaseUnit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
               // Debug.Log($"{ControlledUnit.GetFullName} selected unit {value}");
                _selectedUnit = value;
                wasSelectedUnitUpdated = false;
            }
        }
        public DummyUnit FocusUnit { get; set; }
        public PlayerUnit Player { get; set; } // if it is set it means the unit is in combat

        private void OnUpdatedUnit() { wasSelectedUnitUpdated = true; }


        public event StateMachineEvent<UnitActionType> AgressiveActionRequestSM;
        public event StateMachineEvent<UnitActionType> ChangeRangeActionRequestSM;
        public event StateMachineEvent<PlayerUnit> PlayerSpottedSM;
        public event StateMachineEvent<ReferenceUnitType> RequestFocusSM;
        public event StateMachineEvent AggroRequestedSM;
        public event StateMachineEvent RotationRequestedSM;

        public event SimpleEventsHandler<bool, IManagedComponent> ComponentChangedStateToEvent; // unused

        public NavMeshAgent NMAgent { get; }
        [HideInInspector] public Transform[] PatrolPoints;
        [HideInInspector] public int CurrentPatrolPointIndex = 0;

        public Transform EyesEmpty { get; private set; } // used for sphere casting to look

        #region setups
        public StateMachine(NavMeshAgent agent, EnemyStatsConfig stats, State init, State dummy, ControlledUnit unit)
        {
            NMAgent = agent;
            GetEnemyStats = stats;
            CurrentState = init;
            RemainState = dummy;
            ControlledUnit = unit;
        }
        //public void SetAI(bool setting)
        //{
        //    aiActive = setting;
        //    ComponentChangedStateToEvent?.Invoke(setting, this);

        //    if (NMAgent == null) return;
        //    NMAgent.isStopped = !setting;
        //}

        public void TransitionToState(State nextState)
        {
            if (nextState != RemainState)
            {
#if UNITY_EDITOR
                if (_debugStates) Debug.Log($"{ControlledUnit} switched states: {CurrentState} -> {nextState}, time elapsed: {TimeInState}");
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
            Physics.SphereCast(EyesEmpty.position, GetEnemyStats.LookSphereRadius, EyesEmpty.forward, out var hit, GetEnemyStats.LookRange);
            if (hit.collider == null) return false;
            var _coll = hit.collider;

            var result = _coll.CompareTag("Player");
            if (result)
            {
                PlayerSpottedSM?.Invoke(_coll.GetComponent<PlayerUnit>());
                Player = _coll.GetComponent<PlayerUnit>();
            }
            return result;
        }
        public void OnAttackRequest(UnitActionType type)
        {
            AgressiveActionRequestSM?.Invoke(type);
        }
        public void OnSetFocus(ReferenceUnitType type) => RequestFocusSM?.Invoke(type);
        public bool CheckIsInStoppingRange()
        {
            var result = Vector3.Distance(NMAgent.transform.position, NMAgent.destination) < NMAgent.stoppingDistance;
            return result;
        }
        public void OnAggroRequest() => AggroRequestedSM?.Invoke();
        public bool OnAllyNeedsHelp()
        {
            if (FocusUnit == null) return false;
            else return true;
           // return FocusUnit.GetInputs().AssessStat(AffectedStat.Health).GetCurrent / FocusUnit.GetInputs().AssessStat(AffectedStat.Health).GetMax <= 0.5f;  // HUGE todo
        }
        public void OnRotateRequest() => RotationRequestedSM?.Invoke();
        public void OnSwapRanges(UnitActionType type) => ChangeRangeActionRequestSM?.Invoke(type);

        public void Ping()
        { }

    }
}