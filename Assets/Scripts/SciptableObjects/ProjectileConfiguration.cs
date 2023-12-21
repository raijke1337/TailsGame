using System;
using UnityEngine;
namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Items/Projectile")]
    public class ProjectileConfiguration : ScriptableObject
    {

        public ProjectileComponent ProjectilePrefab;
        public ProjectileSettingsPackage Settings;

    }
    [Serializable] public class ProjectileSettingsPackage
    {
        public float TimeToLive;
        public float ProjectileSpeed;
        [Range(1, 999), Tooltip("How many enemies will be hit by this projectile")] public int ProjectilePenetration;
    }


}