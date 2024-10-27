using Arcatech.AI;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Triggers;
using Arcatech.Units.Behaviour;
using Arcatech.Units.Inputs;
using ECM.Controllers;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Arcatech.BlackboardSystem;

namespace Arcatech.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCUnit : ControlledUnit, IExpert
    {
        public RoomUnitsGroup UnitsGroup { get; set; }

        //used by room ctrl
        public event UnityAction<NPCUnit> OnUnitAttackedEvent = delegate { };

        [Space, Header("Idling settings")]
        protected Transform _player;
        [SerializeField] protected float _playerDetectionRange = 8f;
        [SerializeField] float _idleWanderRange = 5f;
        [SerializeField] float _waitAtIdleSpotTime = 3f;

        protected virtual void OnDrawGizmos()
        {
            if (_showDebugs)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, _playerDetectionRange);
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (this.Side == Side.EnemySide && !CompareTag("Enemy"))
            {
                Debug.LogError($"Set enemy tag for {GetName}");
            }
        }
        protected override void HandleDamage(float value)
        {
            if (bb == null)
            {
                bb = UnitsManager.Instance.GetBlackboard;
            }

            OnUnitAttackedEvent?.Invoke(this);
            bb.SetValue(groupCombat, true);


            base.HandleDamage(value);
        }

        protected override void HandleInteractionAction(IInteractible i)
        {
            if (_showDebugs) Debug.Log($"{GetName} wants to interact with {i}");
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
            _player = FindObjectOfType<PlayerUnit>().transform;
            BaseBehaviourSetup();
            SetupBehavior();
        }

        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            _ground.DetectGround();

            _animator.SetBool("isMoving", agent.velocity.magnitude > 0 && !agent.isStopped);
            ExecuteBehaviour();
        }

        #region behavior


        protected BehaviourTree tree;
        [Space, SerializeField, Self] protected NavMeshAgent agent;


        protected float initStoppingDistance;
        protected Blackboard bb;
        protected BlackboardKey groupCombat;
        protected BlackboardKey safeSpot;

        void BaseBehaviourSetup()
        {
            tree = new BehaviourTree(GetName + " undefined behavior tree");
            bb = UnitsManager.Instance.GetBlackboard;
            groupCombat = bb.GetOrRegisterKey("groupCombat");
            safeSpot = bb.GetOrRegisterKey("safeSpotLocation");


            initStoppingDistance = agent.stoppingDistance;
        }
        protected virtual void SetupBehavior()
        {
            BehaviourPrioritySelector actionPicker = new BehaviourPrioritySelector("generic npc activity");
            actionPicker.AddChild(SetupIdleRoaming());
            actionPicker.AddChild(SetupCowardlyBehaviour());
            tree.AddChild(actionPicker);
        }
        protected virtual void ExecuteBehaviour()
        {
            if (ActionLock || UnitPaused) return;            
            if (tree?.Process(this) == Node.NodeStatus.Fail)
            {
                Debug.Log($"{GetName} has an empty behavior tree!");
            }
        }
        /// <summary>
        /// priority 0
        /// </summary>
        /// <returns>0 priority idling sequence</returns>
        protected Sequence SetupIdleRoaming()
        {
            Sequence roamandWait = new Sequence("walk around and wait",0);
            Leaf resetStoppingDistance = new Leaf(new BehaviourAction(() => agent.stoppingDistance = initStoppingDistance), "Reset stopping distance");
            Leaf roam = new Leaf(new RoamInRangeStrategy(_idleWanderRange, transform.position,agent), $"roam in range {_idleWanderRange}");


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

            roamandWait.AddChild(resetStoppingDistance);
            roamandWait.AddChild(roam);
            roamandWait.AddChild(idleWait);

            return roamandWait;
        }
        /// <summary>
        /// priority 10
        /// </summary>
        /// <returns>run away when attacked sequence</returns>
        protected Sequence SetupCowardlyBehaviour()
        {
            safeSpot = bb.GetOrRegisterKey("safeSpot");
            Sequence runAwayFromAttack = new Sequence("run away to safe spot in combat", 10);
            bool IsCombat()
            {
                if (bb.TryGetValue(groupCombat, out bool yes))
                {
                    if (!yes)
                    {
                        runAwayFromAttack.Reset();
                        return false;
                    }
                }
                agent.speed *= 2; //palceholder
                return true;
            }

            Leaf checkAttacked = new Leaf(new BehaviourCondition(IsCombat), "was attacked sometime ago");
            Vector3 GetSafeSpot ()
            {
                bb.TryGetValue(safeSpot, out Vector3 s);
                return s;
            }

            Leaf runAway = new Leaf(new RoamInRangeStrategy(_idleWanderRange, GetSafeSpot(), agent),
                $"run away to a spot around {GetSafeSpot()}");

            Leaf reset = new Leaf(new BehaviourAction(() => agent.speed /= 2), "reset speed");

            runAwayFromAttack.AddChild(checkAttacked);
            runAwayFromAttack.AddChild(runAway);
            runAwayFromAttack.AddChild(reset);

            return runAwayFromAttack;
        }

        protected bool CheckDistanceToPlayer(float distance)
        {
            return Vector3.Magnitude(transform.position - _player.transform.position) <= distance;
        }


        #region blackboard IExpert code testing

        public virtual int GetActionImportance(Blackboard bb)
        {
            return 1;
        }

        public virtual void Execute(Blackboard bb)
        {
            if (!CheckDistanceToPlayer(_playerDetectionRange))
            {
                bb.SetValue(groupCombat, false);
                agent.speed /= 2;
            }
        }


        #endregion


        #endregion


    }
}

