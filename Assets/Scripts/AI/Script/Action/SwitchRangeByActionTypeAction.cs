using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Action/Switch Range by desired combat action")]

    public class SwitchRangeByActionTypeAction : Action
    {
        public CombatActionType DesiredType;
        public override void Act(StateMachine controller)
        {
            controller.OnSwapRanges(DesiredType);
        }
    }

}