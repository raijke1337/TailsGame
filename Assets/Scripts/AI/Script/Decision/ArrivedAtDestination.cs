using Arcatech.Units.Inputs;
using UnityEngine;
[CreateAssetMenu(menuName = "AIConfig/Decision/ArrivedAtDestination")]
public class ArrivedAtDestination : Decision
{
    public override bool Decide(StateMachine controller)
    {
        var result = controller.CheckIsInStoppingRange();
        if (result) controller.OnPatrolPointReached();
        return result;
    }
}

