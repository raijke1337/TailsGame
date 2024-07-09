using Arcatech.Units.Inputs;
using UnityEngine;
namespace Arcatech.AI
{
    [CreateAssetMenu(menuName = "AIConfig/Action/Switch Range by desired combat action")]

    public class SwitchRangeByActionTypeAction : Action
    {
        public UnitActionType DesiredType;
        public override void Act(EnemyStateMachine controller)
        {
            controller.OnSwapRanges(DesiredType);
        }
    }

}