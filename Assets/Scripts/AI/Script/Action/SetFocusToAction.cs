using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Action/SetFocus")]
    public class SetFocusToAction : Action
    {
        public ReferenceUnitType DesiredType;
        public override void Act(StateMachine controller)
        {
            controller.OnSetFocus(DesiredType);
        }
    }

}