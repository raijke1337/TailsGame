using Arcatech.Actions;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Items/Projectile")]
    public class SerializedProjectileConfiguration : ScriptableObject
    {

        [SerializeField] ProjectileComponent ProjectilePrefab;

        [SerializeField] float TimeToLive;
        [SerializeField] float ProjectileSpeed;


        [SerializeField, Tooltip("Placeholder for homing projectiles, range of scanning for tgts")] float HomingRange = 6f;

        [Range(1, 10), Tooltip("How many enemies will be hit by this projectile"),SerializeField] int ProjectilePenetration;

        [SerializeField] SerializedActionResult[] UnitCollisionResult;  
        [SerializeField] SerializedActionResult[] ExpirationCollisionResult;



        private void OnValidate()
        {
            Assert.IsNotNull(ProjectilePrefab);
            Assert.IsNotNull(UnitCollisionResult);
        }
        /// <summary>
        /// instantiate the prefab and set it
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="place"></param>
        /// <param name="spread">in euler degrees</param>
        /// <returns></returns>
        public virtual ProjectileComponent ProduceProjectile(BaseEntity owner, Transform place,float spread = 0f)
        {
            var proj = Instantiate(ProjectilePrefab, place.position,place.rotation) ;
            proj.Owner = owner;

            Vector3 dir = owner.transform.forward + new Vector3 (UnityEngine.Random.Range(-spread, spread), UnityEngine.Random.Range(-spread, spread), UnityEngine.Random.Range(-spread, spread));

            proj.transform.forward = dir;

            proj.Lifetime = TimeToLive;
            proj.RemainingHits = ProjectilePenetration; 
            proj.Speed = ProjectileSpeed;

            proj.SetResult(UnitCollisionResult,ExpirationCollisionResult);

            if (proj is HomingProjectileComponent h)
            {
                h.WithHoming(HomingRange);
            }

            return proj;
        }

    }

}