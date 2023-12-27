using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Action/Move To:/Unit")]
    internal class MoveToUnitAction : Action
    {
        public override void Act(StateMachine controller)
        {
            //if (controller.NMAgent.isStopped == true) controller.NMAgent.isStopped = false;
            controller.NMAgent.Resume();
            controller.NMAgent.SetDestination(controller.SelectedUnit.transform.position);
        }
    }
}