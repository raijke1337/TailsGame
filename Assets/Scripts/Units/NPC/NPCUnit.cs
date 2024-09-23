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
            base.HandleDeath();
        }
        protected override void OnActionLock(bool locking)
        {
            agent.ResetPath();
        }


        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            SetupBehavior();
        }

        public override void RunUpdate(float delta)
        {
            base.RunUpdate(delta);
            ExecuteBehaviour();
        }



        #region behavior


        protected BehaviourTree tree;
        [Space, SerializeField, Self] protected NavMeshAgent agent;

        protected virtual void SetupBehavior()
        {  
            UnitIsGrounded = true; // placeholder until movement ctrl is implemented
        }
        protected virtual void ExecuteBehaviour()
        {
            if (ActionLock) return;
            tree?.Process(this);
        }

        #endregion


    }

}