using Arcatech.AI;
using Arcatech.Triggers;
using Arcatech.Units.Behaviour;
using KBCore.Refs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Arcatech.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCUnit : ControlledUnit, IRoomUnitTacticsMember
    {

        [Space, Header("Idling settings")]
        protected Transform _player;
        [SerializeField] float _idleWanderRange = 5f;
        [SerializeField] float _waitAtIdleSpotTime = 3f;
        [SerializeField, Range(0, 1)] float _callDistressHealth = 0.1f;

        [Space, Header("Patrol settings, uses idle at spot timer")]
        [SerializeField] protected List<NestedList<Transform>> patrolPointVariants;

        [Space, Header("Player seeking setting")]
        [SerializeField] protected float _playerDetectionSphereCastRange = 8f;
        [SerializeField,Range(0.01f,1f), Tooltip("How often enemy scans for player in front of self")] protected float _sphereCastDelay = 0.1f;
        [SerializeField, Range(1, 10f)] protected float _sphereCastRadius = 3f;
        [SerializeField, Range(1, 999)] protected float _combatTimeout = 5f;

        [Space,SerializeField] SerializedUnitAction _enterCombatAction;
        [SerializeField] SerializedUnitAction _exitCombatAction;

        protected virtual void OnDrawGizmos()
        {
            if (_showDebugs)
            {
                Gizmos.color = Color.yellow;
                if (UnitInCombatState)
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawWireSphere(_headT.position, _sphereCastRadius);
                Gizmos.DrawWireSphere(_headT.position + (_headT.forward * _playerDetectionSphereCastRange), _sphereCastRadius);
                Gizmos.DrawLine(_headT.position, _headT.position + (_headT.forward * _playerDetectionSphereCastRange));
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (this.Side == Side.EnemySide && !CompareTag("Enemy"))
            {
                Debug.LogError($"Set enemy tag for {UnitName}");
            }
        }
        
        protected override void HandleDamage(float value)
        {
            OnUnitAttackedEvent?.Invoke(this);
            UnitInCombatState = true;
            base.HandleDamage(value);
        }

        protected override void HandleInteractionAction(IInteractible i)
        {
            if (_showDebugs) Debug.Log($"{UnitName} wants to interact with {i}");
            i.AcceptInteraction(this);
        }
        protected override void OnActionLock(bool locking)
        {
            agent.isStopped = locking;
            if (locking) _animator.SetBool("isMoving", false);
        }
        protected override void OnUnitPause(bool isPause)
        {
            if (agent != null)
            {
                agent.isStopped = isPause;
            } // wtf....
            if (_animator != null) _animator.SetBool("isMoving", !isPause);
        }

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            agent.speed = movementStats.Stats[Stats.MovementStatType.Movespeed].Max;
            agent.updateRotation = true; // todo check what this adoes

            _player = FindObjectOfType<PlayerUnit>().transform;
            BaseBehaviourSetup();
            SetupBehavior();
        }

        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            _ground.DetectGround();
            InternalCombatStateUpdate(delta);
            _animator.SetBool("isMoving", agent.velocity.magnitude > 0 && !agent.isStopped);
            ExecuteBehaviour();
        }

        #region behavior


        protected BehaviourTree tree;
        [Space, SerializeField, Self] protected NavMeshAgent agent;


        protected float initStoppingDistance;

        void BaseBehaviourSetup()
        {
            tree = new BehaviourTree(UnitName + " undefined behavior tree");

            initStoppingDistance = agent.stoppingDistance;
        }
        protected virtual void SetupBehavior()
        {
            BehaviourPrioritySelector dummyUnitBehavior = new BehaviourPrioritySelector("npc idles");
            if (patrolPointVariants != null && patrolPointVariants.Count > 0)
            {
                dummyUnitBehavior.AddChild(SetupPatrolPoints());
            }
            dummyUnitBehavior.AddChild(SetupIdleRoaming());

            tree.AddChild(dummyUnitBehavior);

            Debug.Log($"{UnitName} is using dummy behavior!");
        }
        void ExecuteBehaviour()
        {
            if (ActionLock || UnitPaused) return;
            if (UnitInCombatState) {

                int i = 0;
            };
            if (tree?.Process(this) == Node.NodeStatus.Fail)
            {
                Debug.Log($"{UnitName} has an empty behavior tree!");
            }
        }
        /// <summary>
        /// priority 0
        /// </summary>
        /// <returns>0 priority idling sequence</returns>
        protected Sequence SetupIdleRoaming()
        {
            Sequence roamandWait = new Sequence("walk around and wait",0);
            Leaf roam = new Leaf(new RoamAroundPoint(_idleWanderRange, transform.position,agent), $"roam in range {_idleWanderRange}");

            Sequence idleWait = new("idle for time");
            Leaf wait = new(new IdleAtPointStrategy(_waitAtIdleSpotTime), $"Wait {_waitAtIdleSpotTime} sec");
            idleWait.AddChild(wait);

            bool isDoneWaiting()
            {
                if (wait.Process(this) == Node.NodeStatus.Success)
                {
                    roamandWait.Reset();
                    return true;
                }
                else return false;
            }
            Leaf checkWait = new Leaf(new BehaviourCondition(() => isDoneWaiting()),"is finished idle time");
            idleWait.AddChild(checkWait);

            roamandWait.AddChild(roam);
            roamandWait.AddChild(idleWait);

            return roamandWait;
        }

        protected Sequence SetupPatrolPoints()
        {
            Sequence randomPatrol = new Sequence("patrol points", 20);
            List<Transform> points = patrolPointVariants[0].InternalList; // todo pick random

            randomPatrol.AddChild(new Leaf(new PatrolPointsStrategy(transform, agent, points, _waitAtIdleSpotTime), "patrol points", 30));

            return randomPatrol;
        }

        protected bool CheckDistanceToPlayer(float distance)
        {
            return Vector3.Magnitude(transform.position - _player.transform.position) <= distance;
        }

        #endregion

        #region room tactics

        protected RoomUnitsGroup _group;
        public void SetUnitsGroup(RoomUnitsGroup g) => _group = g;  

        #region combat state
        public event UnityAction<NPCUnit> OnUnitAttackedEvent = delegate { };
        protected CountDownTimer combatTimeoutTimer;
        bool _inCombat = false;
        string _debugString;

        public bool UnitInCombatState
        {
            get
            {
                return _inCombat;
            }
            set
            {
                if (_inCombat == value) return;
                OnCombatStateChanged(value);
                _inCombat = value;
                if (_showDebugs) Debug.Log($"{UnitName} combat state: {value}");
            }
        }

        protected virtual void OnCombatStateChanged(bool state)
        {
            if (state)
            {
                if (combatTimeoutTimer == null) combatTimeoutTimer = new CountDownTimer(_combatTimeout);
                combatTimeoutTimer.Start();
            }
            if (state && _enterCombatAction != null)
            {
                ForceUnitAction(_enterCombatAction.ProduceAction(this, _headT));
            }
            if (!state && _exitCombatAction != null)
            {
                ForceUnitAction(_exitCombatAction.ProduceAction(this, _headT));
            }
        }

        void InternalCombatStateUpdate(float d)
        {
            SeekPlayer(d);
            combatTimeoutTimer?.Tick(d);
            if (combatTimeoutTimer != null)
            {
                if (combatTimeoutTimer.IsReady)
                {
                    combatTimeoutTimer.Reset(); UnitInCombatState = false; Debug.Log($"combat timeout {UnitName}");
                };
            }
        }
        RaycastHit[] hits = new RaycastHit[20];

        float _castDelay = 0f;
        void SeekPlayer(float delta)
        {
            _castDelay += delta;
            if (_castDelay >= _sphereCastDelay)
            {
                _castDelay = 0f;
                if (Physics.SphereCastNonAlloc(_headT.position, _sphereCastRadius, transform.forward, hits, _playerDetectionSphereCastRange) > 0)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider != null && hit.collider.CompareTag("Player"))
                        {
                            combatTimeoutTimer?.Reset();
                            UnitInCombatState = true;
                            break;
                        }
                    }
                }
            }
        }


        #endregion
        #region UnitNeedsHelp


        public bool UnitNeedsHelp
        {
            get
            {
                return _stats.GetStatValues[BaseStatType.Health].GetPercent <= _callDistressHealth;
            }
        }


        #endregion

        #endregion
    }
}

