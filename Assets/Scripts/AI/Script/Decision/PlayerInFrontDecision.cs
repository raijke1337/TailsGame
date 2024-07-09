using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Decision/IsPlayerInSphereCast")]
    public class PlayerInFrontDecision : Decision
    {
        public override bool Decide(EnemyStateMachine controller)
        {
            return controller.OnLookSphereCast();
        }
    }

}