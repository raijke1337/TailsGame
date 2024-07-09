using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Action/Attack")]
    public class AttackAction : Action
    {
        public UnitActionType CombatActionType;
        public override void Act(EnemyStateMachine controller)
        {
            controller.OnAttackRequest(CombatActionType);
        }

    }

}