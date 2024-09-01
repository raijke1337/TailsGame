using Arcatech.Actions;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "Burst Fire Ranged Weapon Use Strategy", menuName = "Items/Weapon usage strategy/Burst fire Ranged Weapon Use")]
    public class SerializedBurstFireRangedWeaponUseStrategy : SerializedRangedWeaponUseStrategy
    {
        [SerializeField] int _shotsPerBurst;
        public override WeaponStrategy ProduceStrategy(DummyUnit unit, WeaponSO cfg, BaseWeaponComponent comp)
        {
            return new BurstFireRangedWeaponStrategy(_shotsPerBurst, ActionResult ,Action, unit, cfg, TotalCharges, ChargeRestoreTime, InternalCooldown, comp);
        }
    }
    public class BurstFireRangedWeaponStrategy : RangedWeaponStrategy
    {
        int _shotsPerBurst;

        public BurstFireRangedWeaponStrategy(int shots, SerializedActionResult[] results, SerializedUnitAction act, DummyUnit unit, WeaponSO cfg, int charges, float reload, float intcd, BaseWeaponComponent comp) : base(results, act, unit, cfg, charges, reload, intcd, comp)
        {
            _shotsPerBurst = shots;
        }

        protected override void DoShots()
        {
            Owner.StartCoroutine(ShootingCoroutine(_shotsPerBurst));
        }
    }

}