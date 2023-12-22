using Arcatech.Units;
using System;
using UnityEngine;
namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Items/Projectile")]
    public class ProjectileConfiguration : ScriptableObject
    {

        public ProjectileComponent ProjectilePrefab;
        public ProjectileSettingsPackage Settings;

        public ProjectileComponent GetProjectile(BaseUnit owner)
        {

            var c = Instantiate(ProjectilePrefab);
            c.Setup(Settings, owner);
            return c;
    }
    }
    [Serializable]
    public class ProjectileSettingsPackage
    {
        public float TimeToLive;
        public float ProjectileSpeed;
        [Range(1, 999), Tooltip("How many enemies will be hit by this projectile")] public int ProjectilePenetration;
    }


}