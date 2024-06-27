using Arcatech.Items;
using System;
using UnityEngine;

namespace Arcatech
{
    [Serializable]
    public class RangedWeaponConfig
    {

        public SerializedProjectileConfiguration Projectile;

        [Space,SerializeField, Range(1, 20), Tooltip("How projectiles will be spawned until reload is started")] public int Ammo;
        [SerializeField, Range(1, 12), Tooltip("How many projectiles are created per each use")] public int ShotsPerUse;
        [SerializeField, Range(0, 5), Tooltip("Time in reload")] public float Reload;
        [SerializeField, Range(0, 1), Tooltip("Spread of shots (inaccuaracy)")] public float Spread;
    }





}