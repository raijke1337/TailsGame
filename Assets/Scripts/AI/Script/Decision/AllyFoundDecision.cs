using UnityEngine;
[CreateAssetMenu(menuName = "AIConfig/Decision/AllyFound")]
public class AllyFoundDecision : Decision
{
    public override bool Decide(StateMachine controller)
    {
        if (controller.FocusUnit == null) return false;
        return controller.FocusUnit.Side == controller.StateMachineUnit.Side;
    }
}

