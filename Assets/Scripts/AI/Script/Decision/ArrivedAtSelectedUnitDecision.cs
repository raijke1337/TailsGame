using Arcatech.Units.Inputs;
using UnityEngine;
[CreateAssetMenu(menuName = "AIConfig/Decision/ArrivedAtDestination")]
public class ArrivedAtSelectedUnitDecision : Decision
{
    public override bool Decide(StateMachine controller)
    {
        return controller.CheckIsInStoppingRange();
    }
}

