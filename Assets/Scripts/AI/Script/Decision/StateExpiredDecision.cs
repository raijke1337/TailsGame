using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Decision/StateExpired")]
    public class StateExpiredDecision : Decision
    {
        public override bool Decide(EnemyStateMachine controller)
        {
            return (controller.TimeInState >= controller.CurrentState.StateExpiryTime);
        }
    }

}