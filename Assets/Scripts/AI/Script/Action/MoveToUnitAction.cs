using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
