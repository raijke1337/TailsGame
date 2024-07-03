using Arcatech.Triggers;
using Arcatech.Units;

namespace Arcatech.Items
{
    public abstract class BaseWeaponUseStrategy : IWeaponUseStrategy
    {
        public BaseWeaponUseStrategy(DummyUnit unit, SerializedStatsEffectConfig[] effs)
        {
            Owner = unit;
            EffectConfigs = effs;
        }
        public SerializedStatsEffectConfig[] EffectConfigs { get; }
        protected DummyUnit Owner;
        public abstract void WeaponUsedStateEnter();
        public abstract void WeaponUsedStateExit();
    }


}