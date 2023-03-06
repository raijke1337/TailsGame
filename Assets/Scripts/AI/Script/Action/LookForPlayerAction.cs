using UnityEngine;

namespace Assets.Scripts.AI.Script
{
    [CreateAssetMenu(menuName = "AIConfig/Action/LookForPlayer")]
    public class LookForPlayerAction : Action
    {
        public override void Act(StateMachine controller)
        {
            controller.OnLookSphereCast();
        }
    }
}