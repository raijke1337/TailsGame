using JetBrains.Annotations;
using KBCore.Refs;
using UnityEngine.Assertions;

namespace Arcatech.Items
{
    public class RangedWeaponComponent : BaseWeaponComponent
    {
        public  SerializedProjectileConfiguration Projectile;
        protected override void OnValidate()
        {
            base.OnValidate();
            Assert.IsNotNull(Projectile);
        }
    }

}