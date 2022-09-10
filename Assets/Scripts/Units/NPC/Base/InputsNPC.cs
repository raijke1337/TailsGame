using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.Reflection;
using System;
using System.Text;

[RequireComponent(typeof(NavMeshAgent), typeof(Collider), typeof(Rigidbody))]
public abstract class InputsNPC : ControlInputsBase
{
#if UNITY_EDITOR
    [SerializeField, Tooltip("Show action data in console")] public bool DebugEnabled = false;
#endif

    #region fields

    public RoomController UnitRoom { get; set; }
    [SerializeField] protected List<Transform> patrolPoints;
    
    public void SwitchState(bool setting) => _stateMachine.SetAI(setting);
    [Space,SerializeField] protected State InitialState;
    [SerializeField] protected State DummyState;
    [SerializeField] protected State CurrentState;

    protected EnemyStats _enemyStats;
    protected NavMeshAgent _navMeshAg;
    [Space,SerializeField] protected StateMachine _stateMachine;
    public override UnitType GetUnitType() => _enemyStats.EnemyType;

    #endregion
    #region init

    public override void InitControllers(string stats)
    {
        base.InitControllers(stats);
        //_weaponCtrl = new EnemyWeaponCtrl(Empties); // todo? bandaid
    }

    public override void BindControllers(bool isEnable)
    {
        base.BindControllers(isEnable);

        if (patrolPoints.Count == 0)
        {
            patrolPoints.Add(transform);
        }

        _enemyStats = new EnemyStats(Extensions.GetConfigByID<EnemyStatsConfig>(Unit.GetID));


        _navMeshAg = GetComponent<NavMeshAgent>();

        _navMeshAg.speed = _statsCtrl.GetBaseStats[BaseStatType.MoveSpeed].GetCurrent;
        _navMeshAg.stoppingDistance = _enemyStats.AttackRange;
        _stateMachine = new StateMachine(_navMeshAg, _enemyStats, InitialState, DummyState, Unit);

        Bind(isEnable);
      _stateMachine.SetPatrolPoints(patrolPoints);
    }

    protected virtual void Bind(bool isStart)
    {
#if UNITY_EDITOR
       //if (DebugEnabled && isStart) ReflexionBinds(isStart);
#endif

        if (isStart)
        {
            _handler.RegisterUnitForStatUpdates(_stateMachine);
            _stateMachine.AgressiveActionRequestSM += Fsm_AgressiveActionRequestSM;
            _stateMachine.PlayerSpottedSM += Fsm_PlayerSpottedSM;
            _stateMachine.RequestFocusSM += Fsm_GetFocusUnitSM;
            _stateMachine.AggroRequestedSM += Fsm_AggroRequestedSM;
            _stateMachine.RotationRequestedSM += Fsm_RotationRequestedSM;
            _stateMachine.ChangeRangeActionRequestSM += Fsm_ChangeRangeActionRequestSM;
        }
        else
        {
            _handler.RegisterUnitForStatUpdates(_stateMachine, false);
            _stateMachine.AgressiveActionRequestSM -= Fsm_AgressiveActionRequestSM;
            _stateMachine.PlayerSpottedSM -= Fsm_PlayerSpottedSM;
            _stateMachine.RequestFocusSM -= Fsm_GetFocusUnitSM;
            _stateMachine.AggroRequestedSM -= Fsm_AggroRequestedSM;
            _stateMachine.RotationRequestedSM -= Fsm_RotationRequestedSM;
            _stateMachine.ChangeRangeActionRequestSM -= Fsm_ChangeRangeActionRequestSM;
        }
    }

    #endregion
    protected virtual void LateUpdate()
    {
        // todo too many bandaids
        if (_stateMachine == null) return;
        CurrentState = _stateMachine.CurrentState;
        velocityVector = _stateMachine.CurrentVelocity;

#if UNITY_EDITOR
        if (DebugEnabled)
        {
            if (UnitRoom == null) Debug.LogError($"{this} has no room assigned");
            _stateMachine.Debugging = DebugEnabled;
        }
        #endif
    }


    #region reflection excercise

    protected virtual void ReflexionBinds(bool isStart)
    {
        // here we find all events in class and sub to them
        string prefix = Constants.StateMachineData.c_MethodPrefix;
        // get methods from inputs
        Type _inputs = typeof(InputsNPC);
        MethodInfo[] responses = _inputs.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic);

        // filter by prefix to only get proper methods
        List<MethodInfo> filtered = new List<MethodInfo>();
        foreach (var r in responses)
        {
            bool result;
            result = r.Name.Contains(Constants.StateMachineData.c_MethodPrefix);
            if (result)
            {
                filtered.Add(r);
            }
        }

        List<(EventInfo, MethodInfo, Delegate)> values = new List<(EventInfo, MethodInfo, Delegate)>();

        // get event data from state machine 
        Type _fsm = typeof(StateMachine);
        var events = _fsm.GetEvents();
        
        // extablish desired methods and delegates and record into tuples
        foreach (var @event in events)
        {
            string desiredMethod = string.Concat(prefix, @event.Name);
            MethodInfo found = null;
            foreach (var method in filtered)
            {
                if (method.Name == desiredMethod) found = method;
            }
            if (found == null) Debug.LogWarning($"{_fsm.Name}{@event.Name} has no appropriate method {desiredMethod} in {this}");
            else
            {
                var delType = @event.EventHandlerType;
                var argument = found.GetParameters();

                Delegate del = Delegate.CreateDelegate(delType,argument.First(),found);

                values.Add((@event, found, del));
            }
        }

        //sub here

        if (isStart)
        {
            foreach (var record in values)
            {
                record.Item1.AddEventHandler(_stateMachine, record.Item3);
                Debug.Log($"Subbing event {record.Item1} to {record.Item2}, delegate is {record.Item3}");
            }
        }
        else
        {
            foreach (var record in values)
            {
                record.Item1.RemoveEventHandler(_stateMachine, record.Item3);
                Debug.Log($"Unubbing event {record.Item1} to {record.Item2}, delegate is {record.Item3}");
            }
        }
    }

    #endregion

    #region state machine

    protected virtual void Fsm_RotationRequestedSM()
    {
        RotateToSelectedUnit();
    }


    protected virtual void Fsm_ChangeRangeActionRequestSM(CombatActionType arg)
    {
        Debug.Log($"{Unit.GetFullName} used switch ranges for {arg} but it has no logic in {this}");
    }

    protected virtual void Fsm_AggroRequestedSM()
    {
        _stateMachine.SelectedUnit = UnitRoom.GetUnitForAI(UnitType.Player);
    }

    protected virtual void Fsm_PlayerSpottedSM(PlayerUnit arg)
    {
        (Unit as NPCUnit).OnUnitSpottedPlayer();
        _stateMachine.SelectedUnit = arg;
    }


    //attack action logic is here
    protected virtual void Fsm_AgressiveActionRequestSM(CombatActionType type)
    {
        bool success;
        string text;

        switch (type)
        {
            case CombatActionType.Melee:
                success = _weaponCtrl.UseWeaponCheck(EquipItemType.MeleeWeap, out text);
                if (success) CombatActionSuccessCallback(type);
                break;
            case CombatActionType.Ranged:
                success = _weaponCtrl.UseWeaponCheck(EquipItemType.RangedWeap, out text);
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
        LerpRotateToTarget(_stateMachine.SelectedUnit.transform.position);
    }
    protected virtual void Fsm_GetFocusUnitSM(UnitType type)
    {
        if (type != UnitType.Self) _stateMachine.FocusUnit = UnitRoom.GetUnitForAI(type);
        else if (type == UnitType.Self) _stateMachine.FocusUnit = Unit;

        if (_stateMachine.FocusUnit != null) _stateMachine.FocusUnit.BaseUnitDiedEvent += Unsub;
    }
    protected void Unsub(BaseUnit unit)
    {
        if (_stateMachine.SelectedUnit == unit) _stateMachine.SelectedUnit = null;
        unit.BaseUnitDiedEvent -= Unsub;
    }
    #endregion

    #region room manager

    public void ForceCombat(PlayerUnit player) => _stateMachine.SelectedUnit = player;

    #endregion


    protected virtual void OnDisable()
    {
        Bind(false);
    }








#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!DebugEnabled || _stateMachine == null || CurrentState == null) return;
        // state gizmos
        Gizmos.color = CurrentState.StateGizmoColor;
        Gizmos.DrawSphere(_stateMachine.EyesEmpty.position, 0.1f);
        Gizmos.DrawLine(_stateMachine.EyesEmpty.position,_stateMachine.EyesEmpty.position+_stateMachine.EyesEmpty.forward*_enemyStats.LookSpereCastRange);
        //navmesh gizmos
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_stateMachine.NMAgent.transform.position, _stateMachine.NMAgent.transform.position+_stateMachine.NMAgent.transform.forward);
    }
#endif







}

