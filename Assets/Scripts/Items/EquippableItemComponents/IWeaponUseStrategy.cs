using Arcatech.Triggers;
using Arcatech.Units;

namespace Arcatech.Items
{
    public interface IWeaponUseStrategy : IStrategy
    {
        SerializedStatsEffectConfig[] EffectConfigs { get; }
        void WeaponUsedStateEnter();
        void WeaponUsedStateExit();
    }

}