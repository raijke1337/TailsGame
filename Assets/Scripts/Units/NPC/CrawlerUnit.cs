using Arcatech.BlackboardSystem;
using Arcatech.Units.Behaviour;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.Units
{
    public class CrawlerUnit : NPCUnit
    {

        [Header("NPC Behaviour : Crawler")]
        [SerializeField, Range(0, 1)] float selfDestructHealthPercent = 0.2f;

        protected override void SetupBehavior()
        {
            BehaviourPrioritySelector actionsPriority = new BehaviourPrioritySelector("actions list " + UnitName);

            Sequence explode = new Sequence("Explode at low hp", 100);

            Leaf checkHP = new Leaf(new BehaviourCondition(()=> _stats.GetStatValue(BaseStatType.Health).GetPercent < selfDestructHealthPercent), "is hp low for skill");
            Leaf useSkill = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.MeleeSkill)), "melee skill");

            explode.AddChild(checkHP);
            explode.AddChild(useSkill);

            Sequence inCombat = new Sequence("combat activity",50);

            Leaf checkCombatState = new Leaf(new BehaviourCondition(() => UnitInCombatState == true), "check combat state");
            Leaf chase = new Leaf(new MoveToTransformStrategy(agent, _player), "move to player");
            Leaf attack = new Leaf(new BehaviourAction(() => HandleUnitAction(UnitActionType.Melee)), "use melee attack");
            Leaf chaseComplete = new Leaf(new BehaviourAction(() => inCombat.Reset()), "reset chase");

            inCombat.AddChild(checkCombatState);
            inCombat.AddChild(chase);
            inCombat.AddChild(attack);
            inCombat.AddChild(chaseComplete);

            actionsPriority.AddChild(explode);
            actionsPriority.AddChild(inCombat);

            if (patrolPointVariants != null && patrolPointVariants.Count > 0) actionsPriority.AddChild(SetupPatrolPoints());
            actionsPriority.AddChild(SetupIdleRoaming());

            tree.AddChild(actionsPriority);
        }
    }
}