using Arcatech.Triggers;
using Arcatech.Units;

namespace Arcatech.Items
{
    public abstract class BaseWeaponUseStrategy : IWeaponUseStrategy
    {
        public abstract SerializedStatsEffectConfig[] EffectConfigs { get; }
        protected DummyUnit _owner;
        public abstract void WeaponUsedStateEnter();
        public abstract void WeaponUsedStateExit();
    }


}