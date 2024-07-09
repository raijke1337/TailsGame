using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Decision/ArrivedAtDestination")]
    public class ArrivedAtDestination : Decision
    {
        public override bool Decide(EnemyStateMachine controller)
        {
            var result = controller.CheckIsInStoppingRange();
            if (result) controller.OnPatrolPointReached();
            return result;
        }
    }

}