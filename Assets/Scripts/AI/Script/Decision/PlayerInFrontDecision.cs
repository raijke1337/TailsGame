using Arcatech.Units.Inputs;
using UnityEngine;
[CreateAssetMenu(menuName = "AIConfig/Decision/IsPlayerInSphereCast")]
public class PlayerInFrontDecision : Decision
{
    public override bool Decide(StateMachine controller)
    {
        return controller.OnLookSphereCast();
    }
}

