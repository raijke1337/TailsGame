using Arcatech.Actions;
using Arcatech.Units;
using System.Collections;
using UnityEngine;

namespace Arcatech.Items
{
    public class RangedWeaponStrategy : WeaponStrategy
    {
        public RangedWeaponStrategy(SerializedUnitAction act, SerializedActionResult[] onUse, SerializedActionResult[] onFinishUse, EquippedUnit unit, WeaponSO cfg, int charges, float reload, float intcd, BaseWeaponComponent comp) : base(act, onUse, onFinishUse, unit, cfg, charges, reload, intcd, comp)
        {
        }

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
            yield return new WaitForSeconds(Action.ProduceAction(Owner).GetExitTimeDelay);
            while (shots > 0)
            {
                if (_remainingCharges > 0)
                {
                    shots--;
                    PerformOnStart(Owner, null, WeaponComponent.Spawner);

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