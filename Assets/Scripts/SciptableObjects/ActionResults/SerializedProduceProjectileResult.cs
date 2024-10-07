using Arcatech.Items;
using Arcatech.Units;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Arcatech.Actions
{
    [CreateAssetMenu(fileName = "New produce projectile result", menuName = "Actions/Action Result/PlaceProjectile", order = 1)]
    public class SerializedProduceProjectileResult : SerializedActionResult
    {
        [SerializeField] SerializedProjectileConfiguration Projectile;
        [SerializeField,Range (1,10)] int numberOfProjectiles;
        [SerializeField, Tooltip("seconds before shot is done"),Range(0.1f, 1f)] float shotDelay = 0.3f;
        [SerializeField, Range(0, 10)] float spread;
        [SerializeField, Range(0.1f, 1f)] float intDelay = 0.1f;

        private void OnValidate()
        {
            Assert.IsNotNull(Projectile);
        }
        public override IActionResult GetActionResult()
        {
            return new ProduceProjectileResult(Projectile,numberOfProjectiles,spread,intDelay,shotDelay);
        }

        public override string ToString()
        {
            return $"projectile result : {Projectile}";
        }
    }
    public class ProduceProjectileResult : ActionResult
    {
        SerializedProjectileConfiguration _p;
        int _num;
        float _spread;
        float _delay;
        float _shotDelay;
        public ProduceProjectileResult(SerializedProjectileConfiguration p, int n,float s, float d, float st)
        {
            _p = p;
            _num = n;
            _spread = s;
            _delay = d;
            _shotDelay = st;
        }

        public override void ProduceResult(BaseEntity user, BaseEntity target, Transform place)
        {
            user.StartCoroutine(ShootingCoroutine(user,place));
        }

        IEnumerator ShootingCoroutine(BaseEntity user, Transform place)
        {
            yield return new WaitForSeconds(_shotDelay);
            int done = 0;
            while (done < _num)
            {
                done++;
                _p.ProduceProjectile(user, place, _spread);
                yield return new WaitForSeconds(_delay);
            }
            yield return null;
        }
    }


}