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

        [Space, Header("Dummy NPC Unit")]
        protected Transform _player;


        [SerializeField] float _idleWanderRange = 5f;
        [SerializeField] float _waitAtIdleSpotTime = 3f;

        [Space,Header("Drop NYI"),SerializeField] private ItemSO _drop;
        

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
        protected override void HandleDeath()
        {
            //if (_drop != null)
            //{
            //    var drop = Instantiate(_dropPrefab, transform.position, transform.rotation);
            //    drop.Content = _drop;
            //    GameManager.Instance.GetGameControllers.LevelManager.RegisterNewTrigger(drop, true);
            //}
            //agent.isStopped = true;
            base.HandleDeath();
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
            tree = new BehaviourTree(GetName + " undefined behavior tree");
            SetupBehavior();
        }

        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            _ground.DetectGround();

            _animator.SetBool("isMoving", agent.velocity.magnitude > 0);
            ExecuteBehaviour();
        }



        #region behavior


        protected BehaviourTree tree;
        [Space, SerializeField, Self] protected NavMeshAgent agent;

        protected Blackboard bb;
        protected BlackboardKey groupCombat;
        protected BlackboardKey safeSpot;

        protected virtual void SetupBehavior()
        {
            BehaviourPrioritySelector actionPicker = new BehaviourPrioritySelector("choose activity");
            bb = UnitsManager.Instance.GetBlackboard;          
            
            groupCombat = bb.GetOrRegisterKey("groupCombat");

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
            Sequence runAwayFromAttack = new Sequence("run away when attacked", 10);
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
            Vector3 awayFromPlayerDir = (transform.position - _player.position).normalized * _idleWanderRange;
            Leaf runAway = new Leaf(new RoamInRangeStrategy(_idleWanderRange, awayFromPlayerDir, agent),
                $"run away to a spot around {awayFromPlayerDir}");
            void ResetCombatState()
            {
                bb.SetValue(groupCombat, false);
                agent.speed /= 2; //palceholder
            }
            Leaf resetState = new Leaf(new BehaviourAction(() => ResetCombatState()),"reset combat key");

            runAwayFromAttack.AddChild(checkAttacked);
            runAwayFromAttack.AddChild(runAway);
            runAwayFromAttack.AddChild(resetState);
            
            return runAwayFromAttack;
        }

        #region blackboard IExpert code testing

        public int GetActionImportance(Blackboard bb)
        {
            return 1;
        }

        public void Execute(Blackboard bb)
        {

        }


        #endregion


        #endregion


    }
}

