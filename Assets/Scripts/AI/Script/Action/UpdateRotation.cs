using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{

    [CreateAssetMenu(menuName = "AIConfig/Action/UpdateRotation")]
    public class UpdateRotation : Action
    {
        public override void Act(EnemyStateMachine controller)
        {
            //if (controller.NMAgent.isStopped == true) controller.NMAgent.isStopped = false;

            controller.OnRotateRequest();
        }
    }

}