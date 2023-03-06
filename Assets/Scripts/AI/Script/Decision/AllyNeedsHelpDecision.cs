using UnityEngine;
[CreateAssetMenu(menuName = "AIConfig/Decision/AllyNeedsHelp")]
public class AllyNeedsHelpDecision : Decision
{
    public override bool Decide(StateMachine controller)
    {
        return controller.OnAllyNeedsHelp();
    }
}

