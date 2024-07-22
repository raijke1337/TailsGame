using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;
using UnityEngine.Events;

namespace Arcatech.Items
{
    public abstract class SerializedWeaponUseStrategy : ScriptableObject
    {
        public SerializedUnitAction Action;
        public int TotalCharges;
        public float ChargeRestoreTime;
        public float InternalCooldown;

        public virtual WeaponStrategy ProduceStrategy (DummyUnit unit, WeaponSO cfg,BaseWeaponComponent comp)
        {
            return new WeaponStrategy(Action, unit, cfg,TotalCharges,ChargeRestoreTime, InternalCooldown,comp);   
        }
    }


}