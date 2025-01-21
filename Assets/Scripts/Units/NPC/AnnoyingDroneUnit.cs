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

        [SerializeField, Range(0, 1), Tooltip("Heal ally")] float skillUseTreschold = 0.5f;

        protected override void SetupBehavior()
        {
            BehaviourPrioritySelector actionsPriority = new BehaviourPrioritySelector("actions list " + GetName);

            //Sequence activateSkillSeq = new Sequence("activate skill", 100);
            //bool IsLowHP()
            //{
            //    if (_stats.GetStatValue(BaseStatType.Health).GetPercent < skillUseTreschold)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        activateSkillSeq.Reset();
            //        return false;
            //    }
            //}

            //Leaf checkHP = new Leaf(new BehaviourCondition(IsLowHP), "is hp low for skill");
            //Leaf useSkill = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.MeleeSkill)), "melee skill");

            //activateSkillSeq.AddChild(checkHP);
            //activateSkillSeq.AddChild(useSkill);

            //actionsPriority.AddChild(activateSkillSeq);

            //Sequence chasePlayerSeq = new Sequence("chasing player", 50);
            //bool PlayerInRange()
            //{
            //    if (Vector3.SqrMagnitude(_player.position - transform.position) < detectionRange) // placeholder
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        chasePlayerSeq.Reset();
            //        return false;
            //    }
            //}

            //chasePlayerSeq.AddChild(new Leaf(new BehaviourCondition(PlayerInRange), "is player in range"));
            //chasePlayerSeq.AddChild(new Leaf(new MoveToPointStrategy(agent, _player), "move to player"));
            //chasePlayerSeq.AddChild(new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.Melee)), "use melee attack"));
            //chasePlayerSeq.AddChild(new Leaf(new BehaviourAction(() => chasePlayerSeq.Reset()), "reset chase"));

            //actionsPriority.AddChild(chasePlayerSeq);

            //Sequence inCombat = new Sequence("combat activity",50);
            //Leaf checkCombatState = new Leaf(new BehaviourCondition(() => UnitInCombatState == true), "check combat state");
            //Leaf chase = new Leaf(new MoveToTransformStrategy(agent, _player), "move to player");

            //inCombat.AddChild(checkCombatState);
            //inCombat.AddChild(chase);

            //actionsPriority.AddChild(inCombat);

            Leaf checkCombat = new Leaf(new BehaviourCondition(() => UnitInCombatState == true), "check state");
            Sequence combatActivity = new Sequence("combat!", 50);
            combatActivity.AddChild(checkCombat);

            BehaviourPrioritySelector pickCombatAction = new BehaviourPrioritySelector("choose action", 0);

            Sequence goHealAlly = new Sequence("heal allied unit",100);
            Leaf checkAlly = new Leaf(new BehaviourCondition(() => _stats.GetStatValue(BaseStatType.Health).GetPercent <= skillUseTreschold),"Placeholder check ally hp");
            // TODO use units room manager hehre
            Leaf moveToAlly = new Leaf(new MoveToTransformStrategy(agent, transform),"placeholder move to transform");
            Leaf skill = new Leaf(new BehaviourAction(()=>HandleUnitAction(UnitActionType.RangedSkill)), "use skill");

            goHealAlly.AddChild(checkAlly); 
            goHealAlly.AddChild(moveToAlly);
            goHealAlly.AddChild(skill);

            Sequence attackPlayer = new Sequence("shoot at player", 80);
            Leaf rotate = new Leaf (new RotateToPoint(agent,_player.position,0f),"aim at player");
            Leaf shoot = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.Ranged)), "fire!");


            combatActivity.AddChild(pickCombatAction);

            Sequence idle = new Sequence("idling", 10);

            if (patrolPointVariants != null && patrolPointVariants.Count > 0)
            {
                idle.AddChild(SetupPatrolPoints());
            }
            idle.AddChild(SetupIdleRoaming());
            tree.AddChild(idle);
        }
    }

}