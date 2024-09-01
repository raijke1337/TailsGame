using Arcatech.Units;
using ECM.Components;
using UnityEngine;

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New apply force result ", menuName = "Actions/Action Result/ApplyForce", order = 2)]
    public class SerializedApplyForceResult : SerializedActionResult
    {
        [SerializeField] Vector3 Force;
        public override IActionResult GetActionResult()
        {
            return new ApplyForceResult(Force);
        }
    }

    public class ApplyForceResult : ActionResult
    {
        Vector3 _f;
        public ApplyForceResult (Vector3 force)
        {
            _f = force;
        }
        public override void ProduceResult(BaseUnit user, BaseUnit target, Transform place)
        {
            if (user.TryGetComponent<CharacterMovement>(out var m))
            {
                var calc = user.transform.forward * _f.z;

                m.ApplyImpulse(calc); // maybe TODO

                Debug.Log($"Result: Apply force {calc} of to {user}");
            }
        }
    }



}