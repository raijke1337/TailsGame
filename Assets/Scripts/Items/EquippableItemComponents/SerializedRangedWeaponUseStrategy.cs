using Arcatech.Actions;
using Arcatech.Triggers;
using Arcatech.Units;
using System.Collections;
using UnityEngine;

namespace Arcatech.Items
{
    [CreateAssetMenu(fileName = "Ranged Weapon Use Strategy", menuName = "Items/Weapon usage strategy/Ranged Weapon Use")]
    public class SerializedRangedWeaponUseStrategy : SerializedWeaponUseStrategy
    {
       // [SerializeField] protected SerializedProjectileConfiguration Projectile;
        public override WeaponStrategy ProduceStrategy(DummyUnit unit, WeaponSO cfg, BaseWeaponComponent comp)
        {
            return new RangedWeaponStrategy(ActionResult, Action, unit, cfg, TotalCharges, ChargeRestoreTime, InternalCooldown, comp);
        }
    }
    public class RangedWeaponStrategy : WeaponStrategy
    {
        public RangedWeaponStrategy(SerializedActionResult[] results, SerializedUnitAction act, DummyUnit unit, WeaponSO cfg, int charges, float reload, float intcd, BaseWeaponComponent comp) : base(act, unit, cfg, charges, reload, intcd, comp)
        {
            if (results != null)
            {
                r = new ActionResult[results.Length];
                for (int i = 0; i < results.Length; i++)
                {
                    r[i] = results[i].GetActionResult();
                }
            }

        }

        IActionResult[] r;
        public override bool TryUseUsable(out BaseUnitAction action)
        {
            bool ok = CheckTimersAndCharges();
            action = null;

            if (ok)
            {
                action = Action.ProduceAction(Owner);
                DoShots();
            }
            return ok;
        }
        protected virtual void DoShots()
        {
            Owner.StartCoroutine(ShootingCoroutine());
        }

        protected IEnumerator ShootingCoroutine(int shots = 1)
        {
            while (shots > 0)
            {
                if (_remainingCharges > 0)
                {
                    shots--;
                    foreach (var re in r)
                    {

                        re.ProduceResult(Owner, null, WeaponComponent.transform);
                    }
                    ChargesLogicOnUse();
                    yield return new WaitForSeconds(0.03f); 
                }
                else
                {
                    break;
                }
            }
            yield return null;
        }
    }
}