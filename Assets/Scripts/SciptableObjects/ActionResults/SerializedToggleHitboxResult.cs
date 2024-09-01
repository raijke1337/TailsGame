using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New toggle hitbox result", menuName = "Actions/Action Result/Toggle hitbox", order = 4)]
    public class SerializedToggleHitboxResult : SerializedActionResult
    {
        public override IActionResult GetActionResult()
        {
            return new ToggleHitboxResult();
        }
    }

    public class ToggleHitboxResult : ActionResult
    {

        public override void ProduceResult(BaseUnit user, BaseUnit target, Transform place)
        {
            Debug.Log($"Result: Toggle hitbox NYI");
        }
    }
}