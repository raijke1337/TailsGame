using Arcatech.Items;
using Arcatech.Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New produce projectile result", menuName = "Actions/Action Result/PlaceProjectile", order = 1)]
    public class SerializedProduceProjectileResult : SerializedActionResult
    {
        [SerializeField] SerializedProjectileConfiguration Projectile;
        private void OnValidate()
        {
            Assert.IsNotNull(Projectile);
        }
        public override IActionResult GetActionResult()
        {
            return new ProduceProjectileResult(Projectile);
        }

        public override string ToString()
        {
            return $"projectile result : {Projectile}";
        }
    }
    public class ProduceProjectileResult : ActionResult
    {
        SerializedProjectileConfiguration _p;
        public ProduceProjectileResult(SerializedProjectileConfiguration p)
        {
            _p = p;
        }

        public override void ProduceResult(BaseEntity user, BaseEntity target, Transform place)
        {
            _p.ProduceProjectile(user, place);
        }
    }


}