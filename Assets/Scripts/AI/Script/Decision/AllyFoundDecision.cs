using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Decision/Ally")]
    public class AllyFoundDecision : Decision
    {
        public override bool Decide(StateMachine controller)
        {
            if (controller.FocusUnit == null) return false;
            return controller.FocusUnit.Side == controller.ControlledUnit.Side;
        }
    }

}