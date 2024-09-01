using Arcatech.Actions;
using Arcatech.Triggers;
using Arcatech.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Items/Projectile")]
    public class SerializedProjectileConfiguration : ScriptableObject
    {

        [SerializeField] ProjectileComponent ProjectilePrefab;
        [SerializeField] float TimeToLive;
        [SerializeField] float ProjectileSpeed;
        [Range(1, 10), Tooltip("How many enemies will be hit by this projectile"),SerializeField] int ProjectilePenetration;

        [SerializeField] SerializedActionResult[] UnitCollisionResult;  
        [SerializeField] SerializedActionResult[] ExpirationCollisionResult;  

        public ProjectileComponent ProduceProjectile(BaseUnit owner, Transform place)
        {
            var proj = Instantiate(ProjectilePrefab, place.position,place.rotation) ;
            proj.Owner = owner;
            proj.transform.forward = owner.transform.forward;

            proj.Lifetime = TimeToLive;
            proj.RemainingHits = ProjectilePenetration; 
            proj.Speed = ProjectileSpeed;

            proj.SetResult(UnitCollisionResult,ExpirationCollisionResult);

            return proj;
        }

    }

}