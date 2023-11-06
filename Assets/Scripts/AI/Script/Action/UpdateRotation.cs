using Arcatech.Units.Inputs;
using UnityEngine;

[CreateAssetMenu(menuName = "AIConfig/Action/UpdateRotation")]
public class UpdateRotation : Action
{
    public override void Act(StateMachine controller)
    {
        //if (controller.NMAgent.isStopped == true) controller.NMAgent.isStopped = false;

        controller.OnRotateRequest();
    }
}

