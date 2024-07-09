using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Action/AggroOnPlayer")]
    internal class AggroOnPlayerAction : Action
    {
        public override void Act(EnemyStateMachine controller)
        {
            controller.OnAggroRequest();
        }
    }

}