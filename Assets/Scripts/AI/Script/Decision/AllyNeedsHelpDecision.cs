using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Decision/AllyNeedsHelp")]
    public class AllyNeedsHelpDecision : Decision
    {
        public override bool Decide(EnemyStateMachine controller)
        {
            return controller.OnAllyNeedsHelp();
        }
    }

}