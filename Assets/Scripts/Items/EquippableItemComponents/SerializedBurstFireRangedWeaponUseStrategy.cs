using Arcatech.Actions;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "Burst Fire Ranged Weapon Use Strategy", menuName = "Items/Use strategy/Ranged Weapon Burst Fire")]
    public class SerializedBurstFireRangedWeaponUseStrategy : SerializedRangedWeaponUseStrategy
    {
        [SerializeField] int _shotsPerBurst;
        public override WeaponStrategy ProduceStrategy(EquippedUnit unit, WeaponSO cfg, BaseWeaponComponent comp)
        {
            return new BurstFireRangedWeaponStrategy(_shotsPerBurst, Action, OnActionStart, OnActionComplete, unit, cfg, TotalCharges, ChargeRestoreTime, InternalCooldown, comp);
        }
    }
    public class BurstFireRangedWeaponStrategy : RangedWeaponStrategy
    {
        int _shotsPerBurst;

        public BurstFireRangedWeaponStrategy(int shots, SerializedUnitAction act, SerializedActionResult[] onUse, SerializedActionResult[] onFinishUse, EquippedUnit unit, WeaponSO cfg, int charges, float reload, float intcd, BaseWeaponComponent comp) : base(act, onUse, onFinishUse, unit, cfg, charges, reload, intcd, comp)
        {
            _shotsPerBurst = shots;
        }

        protected override void DoShots()
        {
            Owner.StartCoroutine(ShootingCoroutine(_shotsPerBurst));
        }
    }

}