using Arcatech.AI;
using Arcatech.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
namespace Arcatech.Units.Inputs

{
    [RequireComponent(typeof(NavMeshAgent), typeof(Collider), typeof(Rigidbody))]
    public class InputsNPC : ControlInputsBase
    {


        #region fields

        public RoomUnitsGroup UnitsGroup { get; set; }
        [SerializeField] protected List<Transform> patrolPoints;

        [Space, SerializeField] protected State InitialState;
        [SerializeField] protected State DummyState;
        [SerializeField] protected State CurrentState;

        protected NavMeshAgent _navMeshAg;
        [Space, SerializeField] protected StateMachine _stateMachine;

        [SerializeField] protected EnemyStatsConfig EnemyStats;


        #endregion
        #region managed
        public override void StartController()
        {
            base.StartController();
            if (patrolPoints.Count == 0)
            {
                patrolPoints.Add(transform);
            }
            _navMeshAg = GetComponent<NavMeshAgent>();

            _navMeshAg.speed = _statsCtrl.AssessStat(TriggerChangedValue.MoveSpeed).GetCurrent;
            _navMeshAg.stoppingDistance = EnemyStats.AttackRange;
            _stateMachine = new StateMachine(_navMeshAg, EnemyStats, InitialState, DummyState, Unit);

            StateMachineBinds(true);
            _stateMachine.SetPatrolPoints(patrolPoints);
        }
        public override void StopController()
        {
            base.StopController();
            StateMachineBinds(false);
        }
        public override void UpdateController(float delta)
        {
            if (_stateMachine == null) return;
            _stateMachine.UpdateInDelta(delta);
            CurrentState = _stateMachine.CurrentState;

            base.UpdateController(delta);

        }
        #endregion

        #region state machine
        protected virtual void StateMachineBinds(bool isStart)
        {
#if UNITY_EDITOR
            //if (DebugEnabled && isStart) ReflexionBinds(isStart);
#endif

            if (isStart)
            {
                _stateMachine.StartComp();
                _stateMachine.AgressiveActionRequestSM += Fsm_AgressiveActionRequestSM;
                _stateMachine.PlayerSpottedSM += Fsm_PlayerSpottedSM;
                _stateMachine.RequestFocusSM += Fsm_GetFocusUnitSM;
                _stateMachine.AggroRequestedSM += Fsm_AggroRequestedSM;
                _stateMachine.ChangeRangeActionRequestSM += Fsm_ChangeRangeActionRequestSM;
            }
            else
            {
                _stateMachine.StopComp();
                _stateMachine.AgressiveActionRequestSM -= Fsm_AgressiveActionRequestSM;
                _stateMachine.PlayerSpottedSM -= Fsm_PlayerSpottedSM;
                _stateMachine.RequestFocusSM -= Fsm_GetFocusUnitSM;
                _stateMachine.AggroRequestedSM -= Fsm_AggroRequestedSM;
                _stateMachine.ChangeRangeActionRequestSM -= Fsm_ChangeRangeActionRequestSM;
            }
        }
        //protected virtual void Fsm_RotationRequestedSM()
        //{
        //    RotateToSelectedUnit();
        //}


        protected virtual void Fsm_ChangeRangeActionRequestSM(UnitActionType arg)
        {
            //Debug.Log($"{Unit.GetFullName} used switch ranges for {arg} but it has no logic in {this}");
        }

        protected virtual void Fsm_AggroRequestedSM()
        {
            _stateMachine.SelectedUnit = UnitsGroup.GetUnitForAI(ReferenceUnitType.Player);
        }

        protected virtual void Fsm_PlayerSpottedSM(PlayerUnit arg)
        {    
            _stateMachine.SelectedUnit = arg;
        }
        protected virtual void Fsm_GetFocusUnitSM(ReferenceUnitType type)
        {
            if (type != ReferenceUnitType.Self) _stateMachine.FocusUnit = UnitsGroup.GetUnitForAI(type);
            else if (type == ReferenceUnitType.Self) _stateMachine.FocusUnit = Unit;

            if (_stateMachine.FocusUnit != null) _stateMachine.FocusUnit.BaseUnitDiedEvent += Unsub;
        }

        //attack action logic is here
        protected virtual void Fsm_AgressiveActionRequestSM(UnitActionType type)
        {
            RequestCombatAction(type);
        }

        protected void Unsub(BaseUnit unit)
        {
            if (_stateMachine.SelectedUnit == unit) _stateMachine.SelectedUnit = null;
            unit.BaseUnitDiedEvent -= Unsub;
        }
        #endregion

        #region room manager

        public void ForceCombat()
        {
            _stateMachine.SelectedUnit = UnitsGroup.GetUnitForAI(ReferenceUnitType.Player);
        }

        #endregion


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!DebugMessage || _stateMachine == null || CurrentState == null) return;
            // state gizmos
            Gizmos.color = CurrentState.StateGizmoColor;
            Gizmos.DrawSphere(_stateMachine.EyesEmpty.position, 0.1f);
            Gizmos.DrawLine(_stateMachine.EyesEmpty.position, _stateMachine.EyesEmpty.position + _stateMachine.EyesEmpty.forward * EnemyStats.LookRange);
            //navmesh gizmos
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_stateMachine.NMAgent.transform.position, _stateMachine.NMAgent.transform.position + _stateMachine.NMAgent.transform.forward);
            // aggro range gizmo
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_stateMachine.NMAgent.transform.position, _stateMachine.GetEnemyStats.LookRange); // look sphere range is used to call nearby allies into combat
        }



        protected override void OnLockInputs(bool isLock)
        {

        }

        protected override Vector3 DoHorizontalMovement(float delta)
        {
            return Vector3.zero;
        }

#endif
    }

}