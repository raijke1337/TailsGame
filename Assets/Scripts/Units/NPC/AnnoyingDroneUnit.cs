using Arcatech.Units.Behaviour;
using AYellowpaper.SerializedCollections.KeysGenerators;
using UnityEngine;

namespace Arcatech.Units
{
    public class AnnoyingDroneUnit : NPCUnit
    {

        [Header("NPC Behaviour : Drone")]
        [SerializeField, Range(0,1),Tooltip("Percent of player detection range at which unit tries to run away")] float _runAwayDistance = 0.5f;
        [SerializeField,Tooltip("Heal the entity when it has low hp")] BaseEntity _assistTarget;
        [SerializeField,Tooltip("Heal at  hp %"), Range (0,1)] float _assistHealth;

        protected override void SetupBehavior()
        {
            Leaf resumeAgent = new Leaf(new BehaviourAction(() => agent.isStopped = false), "resume agent in case it was stopped by combat");
            Leaf stopAgent = new Leaf(new BehaviourAction(() => agent.isStopped = true), "stop agent to perform combat");


            Sequence combatSequence = new Sequence("combat actions " + UnitName, 80);
            Leaf checkCombat = new Leaf(new BehaviourCondition(() => UnitInCombatState == true), "check combat state", 100);

            BehaviourPrioritySelector combatPriority = new BehaviourPrioritySelector("select combat action", 0);
            Leaf combatDone = new Leaf(new BehaviourAction(() => combatSequence.Reset()), "Reset combat");

            Sequence aimAndShoot = new Sequence("aim at player and use weapon", 50);

            Leaf checkDistance = new Leaf(new BehaviourCondition(() => IsPlayerFarEnough()), "check distance to plyaer");
            Leaf rotate = new Leaf(new AimAtTransform(agent, _player, 1f), "aim at player");
            Leaf shoot = new Leaf(new CheckCombatAction(_skills, _weapons, UnitActionType.Ranged), "prepare to shoot");
            Leaf shoot2 = new Leaf(new BehaviourAction(()=>HandleUnitAction(UnitActionType.Ranged)),"Shoot");

            aimAndShoot.AddChild(checkDistance);
            aimAndShoot.AddChild(stopAgent);
            aimAndShoot.AddChild(rotate);
            aimAndShoot.AddChild(shoot);
            aimAndShoot.AddChild(shoot2);
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




            if (_assistTarget != null)
            {
                Sequence assistAlly = new Sequence("assist damaged ally", 100);
                Leaf checkSkill = new Leaf(new CheckCombatAction(_skills, _weapons, UnitActionType.RangedSkill), "is skill ready");
                Leaf checkAllyNeedsHelp = new Leaf(new BehaviourCondition(() =>
                {
                    float hpPerc = _assistTarget.GetDisplayValues[BaseStatType.Health].GetPercent;
                    return hpPerc < _assistHealth;
                }), "check if assist necessary");
                Leaf goToAlly = new Leaf(new MoveToTransformStrategy(agent,_assistTarget.transform),"move to ally");
                Leaf useSkill = new Leaf (new BehaviourAction (()=> HandleUnitAction(UnitActionType.RangedSkill)),"use heal");


                assistAlly.AddChild(checkAllyNeedsHelp);
                assistAlly.AddChild(checkSkill);
                assistAlly.AddChild(resumeAgent);
                assistAlly.AddChild(goToAlly);
                assistAlly.AddChild(useSkill);          


               // tree.AddChild(assistAlly);
                //doCombat.AddChild(assistAlly);
            }

            combatSequence.AddChild(checkCombat);
            combatSequence.AddChild(combatPriority);


            Sequence idleSequence = new Sequence("idling", 10);
            BehaviorInverter noCombatState = new BehaviorInverter("no combat state");
            noCombatState.AddChild(checkCombat);

            idleSequence.AddChild(noCombatState);
            if (patrolPointVariants != null && patrolPointVariants.Count > 0)
            {
                idleSequence.AddChild(SetupPatrolPoints());
            }
            idleSequence.AddChild(SetupIdleRoaming());


            tree.AddChild(idleSequence);
            tree.AddChild(combatSequence);

        }


        bool IsPlayerFarEnough()
        {
            float distance = Vector3.Distance(transform.position,_player.transform.position);
            float percent = distance / _playerDetectionSphereCastRange;
            bool result = percent > _runAwayDistance;
            //if (_showDebugs) Debug.Log($"{distance} distance, {_playerDetectionSphereCastRange} range, result {percent}%, comparing to {_runAwayDistance}, returns {result}");
            return result;
        }

        //bool HasUnitsToHeal ()
        //{
        //    return _group.UnitsInDanger(out _assistTarget);
        //}

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (!_showDebugs) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + agent.transform.forward);

        }
    }

}