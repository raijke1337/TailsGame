using Arcatech.BlackboardSystem;
using Arcatech.Managers;
using Arcatech.Units.Behaviour;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace Arcatech.Units
{
    public class AnnoyingDroneUnit : NPCUnit
    {

        [Header("NPC Behaviour : Drone")]
        [SerializeField, Range(0,1),Tooltip("Percent of player detection range at which unit tries to run away")] float _runAwayDistance = 0.5f;


        protected override void SetupBehavior()
        {
            BehaviourPrioritySelector allActions = new BehaviourPrioritySelector("unit actions");

            Sequence goHealAlly = new Sequence("heal allied unit actions", 100);
            Leaf canHelp = new Leaf(new BehaviourCondition(() => _skills.CanUseAction(UnitActionType.RangedSkill)), "check if ranged skill is ready");
            Leaf allyNeedsHeal = new Leaf(new BehaviourCondition(() => _initHelp == true), "check if help sequence init by tactics");
            Leaf moveToAlly = new Leaf(new MoveToTransformStrategy(agent, _unitToHelp), "move to unit set by tactics");
            Leaf skill = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.RangedSkill)), "use skill");
            Leaf helpDone = new Leaf(new BehaviourAction(CompleteHelp), "complete help report to tactics");

            goHealAlly.AddChild(canHelp);
            goHealAlly.AddChild(allyNeedsHeal);
            goHealAlly.AddChild(moveToAlly);
            goHealAlly.AddChild(skill);
            goHealAlly.AddChild(helpDone);



            Sequence doCombat = new Sequence("combat actions " + GetName, 80);
            Leaf checkCombat = new Leaf(new BehaviourCondition(() => UnitInCombatState == true), "check combat state",100);
            Leaf resetCombat = new Leaf(new BehaviourAction(() => doCombat.Reset()),"reset combat seq");

            BehaviourPrioritySelector combatPriority = new BehaviourPrioritySelector("select combat action", 0);


            Sequence fleeFromPlayer = new Sequence("run away from player", 80);
            Leaf checkDistance = new Leaf(new BehaviourCondition(() => CheckDistance()),"check distance to plyaer");
            Leaf flee = new Leaf(new RoamAroundPoint(20f, transform.position, agent),"placeholder pick a point to run to");
            fleeFromPlayer.AddChild(checkDistance);
            fleeFromPlayer.AddChild(flee);
            fleeFromPlayer.AddChild(resetCombat);

            Sequence attackPlayer = new Sequence("shoot at player", 60);
            Leaf rotate = new Leaf (new AimAtTransform(agent,_player,1f),"aim at player");
            Leaf shoot = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.Ranged)), "fire!");
            attackPlayer.AddChild(rotate);
            attackPlayer.AddChild(shoot);
            attackPlayer.AddChild(resetCombat);

            combatPriority.AddChild(fleeFromPlayer);
            combatPriority.AddChild(attackPlayer);
            combatPriority.AddChild(fleeFromPlayer);

            doCombat.AddChild(checkCombat);
            doCombat.AddChild(combatPriority);


            allActions.AddChild(doCombat);


            Sequence idleActivity = new Sequence("idling", 10);
            if (patrolPointVariants != null && patrolPointVariants.Count > 0)
            {
                idleActivity.AddChild(SetupPatrolPoints());
            }
            idleActivity.AddChild(SetupIdleRoaming());
            allActions.AddChild(idleActivity);

            tree.AddChild(allActions);
        }

        bool _initHelp = false;
        bool _helpDone = false;
        Transform _unitToHelp;

        void CompleteHelp()
        {
            _helpDone = true;
            _unitToHelp = null;
             tree.Reset();
        }
        public override void Execute(Blackboard bb)
        {
            if (!_helpDone && bb.gatherAround != null && !_initHelp)
            {
                _initHelp = true;
                _unitToHelp = bb.gatherAround.transform;
            }
            if (_helpDone)
            {
                _initHelp = false;
                _helpDone = false;
                bb.ResetGatherPoint();
            }
            base.Execute(bb);
        }
        public override int GetActionImportance(Blackboard bb)
        {
            if (_helpDone) return 60;
            else return 10;
        
        }

        bool CheckDistance()
        {
            float distance = Vector3.Distance(transform.position,_player.transform.position);
            float percent = distance / _playerDetectionSphereCastRange;
            return (percent < _runAwayDistance);
        }


        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (!_showDebugs) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + agent.transform.forward);

        }
    }

}