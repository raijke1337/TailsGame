using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "Ranged Weapon Use Strategy", menuName = "Strategy/Ranged Weapon Use")]
    public class SerializedRangedWeaponUseStrategy : SerializedWeaponUseStrategy
    {
        [SerializeField] SerializedProjectileConfiguration Projectile;
        public override WeaponStrategy ProduceStrategy(DummyUnit unit, WeaponSO cfg, BaseWeaponComponent comp)
        {
            return new RangedWeaponStrategy(Projectile, Action, unit, cfg, TotalCharges, ChargeRestoreTime, InternalCooldown, comp);
        }
    }
    public class RangedWeaponStrategy : WeaponStrategy
    {
        SerializedProjectileConfiguration ProjectilePrefab { get; }
        public RangedWeaponStrategy(SerializedProjectileConfiguration proj, SerializedUnitAction act, DummyUnit unit, WeaponSO cfg, int charges, float reload, float intcd, BaseWeaponComponent comp) : base(act, unit, cfg, charges, reload, intcd, comp)
        {
            ProjectilePrefab = proj; 
        }

        public override bool TryUseItem(out BaseUnitAction action)
        {
            bool ok =  base.TryUseItem(out action);
            
            if (ok)
            {
                // TODO
                ProjectilePrefab.ProduceProjectile(Owner, WeaponComponent.transform, Config.UseEffects);
            }
            return ok;
        }
    }

}