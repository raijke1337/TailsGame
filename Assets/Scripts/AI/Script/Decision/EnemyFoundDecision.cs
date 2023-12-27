using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Decision/EnemyFound")]
    public class EnemyFoundDecision : Decision
    {
        public override bool Decide(StateMachine controller)
        {
            if (controller.SelectedUnit == null) return false;
            return controller.SelectedUnit.Side != controller.StateMachineUnit.Side;
        }
    }

}