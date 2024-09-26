using Arcatech.AI;
using Arcatech.Items;
using Arcatech.Managers;
using Arcatech.Triggers;
using Arcatech.Units.Behaviour;
using Arcatech.Units.Inputs;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Arcatech.Units
{




    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCUnit : ControlledUnit
    {
        public RoomUnitsGroup UnitsGroup { get; set; }

        //used by room ctrl
        public event UnityAction<NPCUnit> OnUnitAttackedEvent = delegate { };

        [SerializeField] private LevelRewardTrigger _dropPrefab;
        [SerializeField] private ItemSO _drop;
        
        [SerializeField, Self] NPCUnitInputs inputs;

        [Space, SerializeField] protected EnemyStatsConfig _enemyStatsConfig;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (this.Side == Side.EnemySide && !CompareTag("Enemy"))
            {
                Debug.LogError($"Set enemy tag for {GetUnitName}");
            }
        }
        protected override void HandleDamage(float value)
        {
            OnUnitAttackedEvent?.Invoke(this);
            base.HandleDamage(value);
        }
        protected override void HandleDeath()
        {
            if (_drop != null)
            {
                var drop = Instantiate(_dropPrefab, transform.position, transform.rotation);
                drop.Content = _drop;
                GameManager.Instance.GetGameControllers.LevelManager.RegisterNewTrigger(drop, true);
            }
            //agent.isStopped = true;
            base.HandleDeath();
        }
        protected override void OnActionLock(bool locking)
        {
            agent.isStopped = locking;
            if (locking) _animator.SetBool("isMoving", false);
        }
        protected override void OnUnitPause(bool isPause)
        {
            agent.isStopped = isPause;
            if (isPause) _animator.SetBool("isMoving", false);
        }

        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            SetupBehavior();
        }

        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            _animator.SetBool("isMoving", agent.velocity.magnitude > 0);
            ExecuteBehaviour();
        }



        #region behavior


        protected BehaviourTree tree;
        [Space, SerializeField, Self] protected NavMeshAgent agent;

        protected virtual void SetupBehavior()
        {  
            UnitIsGrounded = true; // placeholder until movement ctrl is implemented
            SetupIdleRoaming(); 
        }
        protected virtual void ExecuteBehaviour()
        {
            if (ActionLock || UnitPaused) return;
            
            tree?.Process(this);
        }
        
        void SetupIdleRoaming()
        {
            tree = new BehaviourTree(GetUnitName + " undefined idle behavior");
            Sequence roamandWait = new Sequence("walk around and wait", 0);
            Leaf roam = new Leaf(new RoamInRangeStrategy(5f,transform.position,agent),"roam in range 5", 100);


            Sequence idleWait = new("idle for time");
            Leaf wait = new(new IdleAtPointStrategy(3f), "Wait 3 sec");
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

            tree.AddChild(roamandWait);
        }


        #endregion


    }

}