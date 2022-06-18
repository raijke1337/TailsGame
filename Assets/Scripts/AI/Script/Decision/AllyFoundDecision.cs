using UnityEngine;
[CreateAssetMenu(menuName = "AIConfig/Decision/AllyFound")]
public class AllyFoundDecision : Decision
{
    public override bool Decide(StateMachine controller)
    {
        if (controller.SelectedUnit == null) return false;
        return controller.SelectedUnit.Side == controller.StateMachineUnit.Side;
    }
}

