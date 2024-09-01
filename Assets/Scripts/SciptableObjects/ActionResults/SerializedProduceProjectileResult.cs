using Arcatech.Items;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New produce projectile result", menuName = "Actions/Action Result/PlaceProjectile", order = 1)]
    public class SerializedProduceProjectileResult : SerializedActionResult
    {
        [SerializeField] SerializedProjectileConfiguration Projectile;

        public override IActionResult GetActionResult()
        {
            return new ProduceProjectileResult(Projectile);
        }
    }
    public class ProduceProjectileResult : ActionResult
    {
        SerializedProjectileConfiguration _p;
        public ProduceProjectileResult(SerializedProjectileConfiguration p)
        {
            _p = p;
        }

        public override void ProduceResult(BaseUnit user, BaseUnit target, Transform place)
        {
            _p.ProduceProjectile(user, place);
            Debug.Log($"Placed projectile {_p}");
        }
    }



}