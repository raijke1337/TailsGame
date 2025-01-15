using UnityEngine;
using Arcatech.Units.Behaviour;
using Arcatech.BlackboardSystem;
using Arcatech.Units.Inputs;
namespace Arcatech.Units
{
    public class MetalGolemUnit : NPCUnit
    {
        [Space, Header("Metal golem unit behaviour")]
        [SerializeField,Range(1,180)] float _playerInFrontAngle = 15f;
        [SerializeField,Range(1,20)] float _chargeRange = 5f;

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (_showDebugs)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, _chargeRange);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);
            }
        }

        protected override void SetupBehavior()
        {
            Sequence idling = SetupIdleRoaming();

            BehaviourPrioritySelector actionsPriority = new BehaviourPrioritySelector("golem behavior");
            actionsPriority.AddChild(idling);

            bool CombatCondition ()
            {
                //bb.TryGetValue(groupCombat, out bool comb);
                return (UnitInCombatState || CheckDistanceToPlayer(_playerDetectionRange));
            }

            Sequence combatSequence = new Sequence("in combat with player", 50);
            Leaf checkCombat = new Leaf(new BehaviourCondition(CombatCondition), "is in combat");
            Leaf resetCombat = new Leaf(new BehaviourAction(() => { agent.stoppingDistance = initStoppingDistance; combatSequence.Reset(); }), "reset combat");
            Leaf justChase = new Leaf(new MoveToTransformStrategy(agent, _player.transform, true, _playerInFrontAngle), "run to player to hit");



            combatSequence.AddChild(checkCombat);
            //combatSequence.AddChild(new Leaf(new BehaviourAction(() => bb.SetValue(groupCombat, true)), "change combat state for room"));

            BehaviorSelector chooseAction = new BehaviorSelector("choose between charge and attack");

            Sequence attackInMelee = new Sequence("chase and use melee attacks");

            Leaf checkMeleeRange = new Leaf(new BehaviourCondition(()=>CheckDistanceToPlayer(agent.stoppingDistance)), "check range to player");

            Leaf attack = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.Melee)), "attack!");
            Leaf resetMelee = new Leaf(new BehaviourAction(() => attackInMelee.Reset()), "recheck melee attack range");

            attackInMelee.AddChild(new Leaf(new BehaviourAction(() => agent.stoppingDistance = initStoppingDistance), "reset to initial stopping range"));
            attackInMelee.AddChild(checkMeleeRange);
            attackInMelee.AddChild(justChase); // check rotation first
            attackInMelee.AddChild(attack);
            attackInMelee.AddChild(resetMelee);

            Sequence chasePlayerAndUseCharge = new Sequence("chase and charge");
            Leaf chargeCheck = new Leaf(new BehaviourCondition (() => CanDoAction(UnitActionType.DodgeSkill)),"check if skill is ready");
            Leaf setRange = new Leaf(new BehaviourAction(()=> agent.stoppingDistance = _chargeRange),$"set stopping distance to {_chargeRange}");
            Leaf moveIntoRange = new Leaf(new MoveToTransformStrategy(agent, _player.transform, true, _playerInFrontAngle), "rotate and chase");
            Leaf useCharge = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.DodgeSkill)), "charge");

            chasePlayerAndUseCharge.AddChild(chargeCheck);
            chasePlayerAndUseCharge.AddChild(setRange);
            chasePlayerAndUseCharge.AddChild(moveIntoRange);
            chasePlayerAndUseCharge.AddChild(useCharge);
            chasePlayerAndUseCharge.AddChild(resetCombat);


            chooseAction.AddChild(attackInMelee);
            chooseAction.AddChild(chasePlayerAndUseCharge);
            chooseAction.AddChild(justChase);

            combatSequence.AddChild(chooseAction);

            actionsPriority.AddChild(combatSequence);
            tree.AddChild(actionsPriority);
        }

        //public override int GetActionImportance(Blackboard bb)
        //{
        //    if (bb.TryGetValue(groupCombat, out bool isCombat))
        //    {
        //        if (!isCombat) { return 0; }
        //        else return 20;
        //    }
        //    return 0;
        //}

        //public override void Execute(Blackboard bb)
        //{
        //    bb.AddAction(() => bb.SetValue(safeSpot, transform.position));
        //}
    }
}

