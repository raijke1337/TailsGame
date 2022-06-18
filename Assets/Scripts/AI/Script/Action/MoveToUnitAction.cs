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
        controller.NMAgent.SetDestination(controller.SelectedUnitTransform.position);
    }
}
