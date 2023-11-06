using Arcatech.Units.Inputs;
using UnityEngine;
[CreateAssetMenu(menuName = "AIConfig/Action/Move To:/Point")]
public class MoveToPoint : Action
{
    public override void Act(StateMachine controller)
    {
        //if (controller.NMAgent.isStopped == true) controller.NMAgent.isStopped = false;

        controller.NMAgent.SetDestination(controller.PatrolPoints[controller.CurrentPatrolPointIndex].position);
        controller.NMAgent.Resume();
    }
}

