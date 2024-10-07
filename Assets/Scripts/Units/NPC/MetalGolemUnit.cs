using UnityEngine;
using Arcatech.Units.Behaviour;
namespace Arcatech.Units
{
    public class MetalGolemUnit : NPCUnit
    {
        [Space, Header("Metal golem unit behaviour")]
        [SerializeField,Range(1,180)] float _playerInFrontAngle = 15f;
        [SerializeField,Range(1,20)] float _minRangeCharge = 8f;

        DashJumpMovementController _movement;

        // [SerializeField] float playRotationAt = 0.4f;
        public override void StartControllerUnit()
        {
            base.StartControllerUnit();
            _movement = GetComponent<DashJumpMovementController>();
        }
        protected override void SetupBehavior()
        {
            Sequence idling = SetupIdleRoaming();

            BehaviourPrioritySelector actionsPriority = new BehaviourPrioritySelector("golem behavior");
            actionsPriority.AddChild(idling);

            tree.AddChild(actionsPriority);



            Sequence chasePlayerSeq = new Sequence("chasing player", 50);
            bool PlayerInRange()
            {
                if (Vector3.SqrMagnitude(_player.position - transform.position) < _minRangeCharge) // placeholder
                {
                    return true;
                }
                else
                {
                    chasePlayerSeq.Reset();
                    return false;
                }
            }


            chasePlayerSeq.AddChild(new Leaf(new BehaviourCondition(PlayerInRange), "is player in range"));
            chasePlayerSeq.AddChild(new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.MeleeSkill)), "use dodge"));
            chasePlayerSeq.AddChild(new Leaf(new MoveToPointStrategy(transform, agent, _player), "move to player"));
            chasePlayerSeq.AddChild(new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.Melee)), "use melee attack"));

            chasePlayerSeq.AddChild(new Leaf(new BehaviourAction(() => chasePlayerSeq.Reset()), "reset chase"));

            actionsPriority.AddChild(chasePlayerSeq);
        }

        public override void ApplyForceResultToUnit(float imp, float time)
        {
            _movement.ApplyPhysicalMovementResult(imp, time);
        }
    }
}

