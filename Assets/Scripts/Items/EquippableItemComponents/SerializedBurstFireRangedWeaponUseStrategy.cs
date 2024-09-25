using Arcatech.Actions;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "Burst Fire Ranged Weapon Use Strategy", menuName = "Items/Use strategy/Ranged Weapon Burst Fire")]
    public class SerializedBurstFireRangedWeaponUseStrategy : SerializedRangedWeaponUseStrategy
    {
        [Header("Depreciated, use serialized produce projectile now")]
        [SerializeField,Tooltip("Not used")] int _shotsPerBurst;
        public override WeaponStrategy ProduceStrategy(EquippedUnit unit, WeaponSO cfg, BaseWeaponComponent comp)
        {
            return new BurstFireRangedWeaponStrategy(_shotsPerBurst, Action, unit, cfg, TotalCharges, ChargeRestoreTime, InternalCooldown, comp);
        }
    }
    public class BurstFireRangedWeaponStrategy : RangedWeaponStrategy
    {
        int _shotsPerBurst;

        public BurstFireRangedWeaponStrategy(int shots, SerializedUnitAction act, EquippedUnit unit, WeaponSO cfg, int charges, float reload, float intcd, BaseWeaponComponent comp) : base(act, unit, cfg, charges, reload, intcd, comp)
        {
            _shotsPerBurst = shots;
        }

        //protected override void DoShots()
        //{
        //    Owner.StartCoroutine(ShootingCoroutine(_shotsPerBurst));
        //}
    }

}