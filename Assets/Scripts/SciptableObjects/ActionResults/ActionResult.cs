using Arcatech.Items;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Actions
{
    public abstract class SerializedActionResult : ScriptableObject
    {
        public abstract IActionResult GetActionResult();
    }

    // trigger toggle
    // stat change
    // spawn projectile 
    // movement impulse
    // spawn particle effect
    public abstract class ActionResult : IActionResult
    {
        public abstract void ProduceResult(BaseUnit user, BaseUnit target,Transform place);
    }
}