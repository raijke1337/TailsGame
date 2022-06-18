using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "AIConfig/Action/AggroOnPlayer")]
internal class AggroOnPlayerAction : Action
{
    public override void Act(StateMachine controller)
    {
        controller.SelectedUnit = controller.PlayerFound;
    }
}

