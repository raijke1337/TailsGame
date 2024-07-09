using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI

{
    [CreateAssetMenu(menuName = "AIConfig/Decision/ArrivedAtDestination")]
    public class ArrivedAtSelectedUnitDecision : Decision
    {
        public override bool Decide(EnemyStateMachine controller)
        {
            return controller.CheckIsInStoppingRange();
        }
    }

}