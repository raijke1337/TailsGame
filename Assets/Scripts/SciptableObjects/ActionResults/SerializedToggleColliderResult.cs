using Arcatech.Items;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New toggle melee collider result", menuName = "Actions/Action Result/Toggle melee collider")]
    public class SerializedToggleColliderResult : SerializedActionResult
    {
        [SerializeField] bool ResultingColliderState;
        public override IActionResult GetActionResult()
        {
            return new ToggleColliderResult(ResultingColliderState);
        }
    }
    public class ToggleColliderResult : ActionResult
    {
        bool state;
        public ToggleColliderResult(bool p)
        {
            state = p;
        }

        public override void ProduceResult(BaseUnit user, BaseUnit target, Transform place)
        {
            if (user is ArmedUnit ar && ar.IsArmed(out IWeapon w))
            {
                if (w.UseStrategy is MeleeWeaponStrategy m)
                {
                    m.SwitchCollider(state);
                }
            }
        }
    }
}