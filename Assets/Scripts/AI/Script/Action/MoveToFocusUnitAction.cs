using UnityEngine;
[CreateAssetMenu(menuName = "AIConfig/Action/Move To:/Focus Unit")]
public class MoveToFocusUnitAction : Action
{
    public override void Act(StateMachine controller)
    {
        //if (controller.NMAgent.isStopped == true) controller.NMAgent.isStopped = false; 
        controller.NMAgent.Resume();
        controller.NMAgent.SetDestination(controller.FocusUnit.transform.position);

    }
}
