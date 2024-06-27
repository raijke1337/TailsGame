using Arcatech.EventBus;
using Arcatech.Triggers;
using Arcatech.Units;
using UnityEngine;

namespace Arcatech.Items
{
    public class MeleeWeaponUseStrategy : BaseWeaponUseStrategy
    {
        private WeaponTriggerComponent _trigger;
        public override SerializedStatsEffectConfig[] EffectConfigs { get; }        
        StatsEffect[] currentUseEffects; // to prevent double application



        public MeleeWeaponUseStrategy(MeleeWeaponComponent weapon, SerializedStatsEffectConfig[] effects, DummyUnit owner)
        {
            _trigger = weapon.Trigger;
            EffectConfigs = effects;
            _owner = owner;
            _trigger.UnitHitEvent += HandleBaseUnitHitEvent;
        }

        private void HandleBaseUnitHitEvent(DummyUnit target)
        {
            EventBus<StatsEffectTriggerEvent>.Raise(new StatsEffectTriggerEvent(target, _owner, true,currentUseEffects));
        }

        public override void WeaponUsedStateEnter()
        {
            if (currentUseEffects == null)
            {
                currentUseEffects = new StatsEffect[EffectConfigs.Length];
            }
            for (int i = 0; i < currentUseEffects.Length; i++)  
            {
                currentUseEffects[i] = new StatsEffect(EffectConfigs[i]);
            }
            _trigger.ToggleCollider(true);
        }

        public override void WeaponUsedStateExit()
        {
            _trigger.ToggleCollider(false);
        }

        
    }


}