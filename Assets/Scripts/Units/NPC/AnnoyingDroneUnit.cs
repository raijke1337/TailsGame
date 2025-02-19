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
            Leaf resumeAgent = new Leaf(new BehaviourAction(() => agent.isStopped = false), "resume agent in case it was stopped by combat");
            Leaf stopAgent = new Leaf(new BehaviourAction(() => agent.isStopped = true), "stop agent to perform combat");


            Sequence doCombat = new Sequence("combat actions " + UnitName, 80);
            Leaf checkCombat = new Leaf(new BehaviourCondition(() => UnitInCombatState == true), "check combat state", 100);

            BehaviourPrioritySelector combatPriority = new BehaviourPrioritySelector("select combat action", 0);
            Leaf combatDone = new Leaf(new BehaviourAction(() => doCombat.Reset()), "Reset combat");

            Sequence aimAndShoot = new Sequence("aim at player and use weapon", 50);

            Leaf checkDistance = new Leaf(new BehaviourCondition(() => IsPlayerFarEnough()), "check distance to plyaer");
            Leaf rotate = new Leaf(new AimAtTransform(agent, _player, 1f), "aim at player");
            Leaf shoot = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.Ranged)), "fire!");

            aimAndShoot.AddChild(checkDistance);
            aimAndShoot.AddChild(stopAgent);
            aimAndShoot.AddChild(rotate);
            aimAndShoot.AddChild(shoot);
            aimAndShoot.AddChild(combatDone);


            Sequence runAwayFromPlayer = new Sequence("run away if player too close", 60);
            BehaviorInverter checkDistanceInverter = new BehaviorInverter("player is too close");
            checkDistanceInverter.AddChild(checkDistance);
            Leaf flee = new Leaf(new RoamAroundPoint(20f, transform.position, agent), "placeholder pick a point to run to");

            runAwayFromPlayer.AddChild(checkDistanceInverter);
            runAwayFromPlayer.AddChild(resumeAgent);
            runAwayFromPlayer.AddChild(flee);


            combatPriority.AddChild(aimAndShoot);
            combatPriority.AddChild(runAwayFromPlayer);


            doCombat.AddChild(checkCombat);
            doCombat.AddChild(combatPriority);


            Sequence idleActivity = new Sequence("idling", 10);
            BehaviorInverter noCombatState = new BehaviorInverter("no combat state");
            noCombatState.AddChild(checkCombat);

            idleActivity.AddChild(noCombatState);
            if (patrolPointVariants != null && patrolPointVariants.Count > 0)
            {
                idleActivity.AddChild(SetupPatrolPoints());
            }
            idleActivity.AddChild(SetupIdleRoaming());


            tree.AddChild(idleActivity);
            tree.AddChild(doCombat);

        }


        bool IsPlayerFarEnough()
        {
            float distance = Vector3.Distance(transform.position,_player.transform.position);
            float percent = distance / _playerDetectionSphereCastRange;
            bool result = percent > _runAwayDistance;
            if (_showDebugs) Debug.Log($"{distance} distance, {_playerDetectionSphereCastRange} range, result {percent}%, comparing to {_runAwayDistance}, returns {result}");
            return result;
        }

        bool HasUnitsToHeal ()
        {
            return _group.UnitsInDanger(out _);
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