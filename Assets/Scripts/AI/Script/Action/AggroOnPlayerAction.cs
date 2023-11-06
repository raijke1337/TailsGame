using Arcatech.Units.Inputs;
using UnityEngine;

[CreateAssetMenu(menuName = "AIConfig/Action/AggroOnPlayer")]
internal class AggroOnPlayerAction : Action
{
    public override void Act(StateMachine controller)
    {
        controller.OnAggroRequest();
    }
}

