using Arcatech.Units.Inputs;
using UnityEngine;
[CreateAssetMenu(menuName = "AIConfig/Action/SetFocus")]
public class SetFocusToAction : Action
{
    public UnitType DesiredType;
    public override void Act(StateMachine controller)
    {
        controller.OnSetFocus(DesiredType);
    }
}

