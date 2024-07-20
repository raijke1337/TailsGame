using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine.Events;

namespace Arcatech.Items
{
    public interface IWeaponUseStrategy : IStrategy
    {
        SerializedStatsEffectConfig[] EffectConfigs { get; }
        void WeaponUsedStateEnter();
        void WeaponUsedStateExit();
        public event UnityAction<IWeapon> UsedItemEvent;
    }

}