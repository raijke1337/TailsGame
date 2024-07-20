using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine.Events;

namespace Arcatech.Items
{
    public abstract class BaseWeaponUseStrategy : IWeaponUseStrategy
    {
        public BaseWeaponUseStrategy(DummyUnit unit, SerializedStatsEffectConfig[] effs, IWeapon weapon)
        {
            Owner = unit;
            EffectConfigs = effs;
            w = weapon;
        }

        public SerializedStatsEffectConfig[] EffectConfigs { get; }

        IWeapon w;
        protected DummyUnit Owner;
        private SerializedStatsEffectConfig[] effects;

        public event UnityAction<IWeapon> UsedItemEvent = delegate { };
        protected void CallUpdate() => UsedItemEvent.Invoke(w);
        public abstract void WeaponUsedStateEnter();
        public abstract void WeaponUsedStateExit();
    }


}