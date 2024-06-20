using Arcatech.Units;
using System;
using UnityEngine;
namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Items/Projectile")]
    public class SerializedProjectileConfiguration : ScriptableObject
    {

        public ProjectileComponent ProjectilePrefab;
        public EquipmentType SpawnPlace;
        public float TimeToLive;
        public float ProjectileSpeed;

        [Range(1, 999), Tooltip("How many enemies will be hit by this projectile")] public int ProjectilePenetration;
    }





    public class ProjectileSettingsPackage
    {
        public EquipmentType SpawnPlace;
        public float TimeToLive;
        public float ProjectileSpeed;
        public ProjectileComponent ProjectilePrefab;
        public int ProjectilePenetration;

        public ProjectileSettingsPackage(SerializedProjectileConfiguration cfg)
        {
            SpawnPlace = cfg.SpawnPlace;
            TimeToLive = cfg.TimeToLive;
            ProjectileSpeed = cfg.ProjectileSpeed;
            ProjectilePenetration = cfg.ProjectilePenetration;
            ProjectilePrefab = cfg.ProjectilePrefab;

        }
    }


}